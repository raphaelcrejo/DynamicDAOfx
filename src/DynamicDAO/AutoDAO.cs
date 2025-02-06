using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace DynamicDAO
{
    /// <summary>
    /// Realiza a conexão à uma fonte de dados, define uma instrução SQL, adiciona e remove parâmetros de entrada, e devolve objetos preenchidos.
    /// </summary>
    public class AutoDAO : IDisposable
    {
        internal bool disposed = false;

        private enum TransactionBehaviour
        {
            CommitTransaction = 1,
            RollbackTransaction = 2
        }

        private IDbConnection _connection;
        private IDbCommand _command;
        private IDbTransaction _transaction = null;

        private string _identifier = string.Empty;

        /// <summary>
        /// Realiza o commit ou rollback de uma transação.
        /// </summary>
        /// <param name="transactionBehaviour">Ação a ser realizada pela transação.</param>

        private void ManageTransaction(TransactionBehaviour transactionBehaviour)
        {
            if (_transaction != null)
            {
                switch (transactionBehaviour)
                {
                    case TransactionBehaviour.CommitTransaction:
                        _transaction.Commit();
                        break;
                    case TransactionBehaviour.RollbackTransaction:
                        _transaction.Rollback();
                        break;
                }
            }
        }

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="AutoDAO"/> com propriedades padrão.
        /// </summary>
        /// <param name="connectionString">String de conexão com o banco de dados.</param>
        public AutoDAO(string connectionString)
        {
            ProviderInfo providerInfo = new ProviderInfo
            {
                ConnectionString = connectionString
            };

            _identifier = providerInfo.Identifier;

            _connection = Core.DataObjects.CreateConnection(providerInfo);
            _command = Core.DataObjects.CreateCommand(_connection, providerInfo);

            if (providerInfo.LockTransaction == true)
            {
                _transaction = Core.DataObjects.CreateTransaction(_connection, providerInfo);
            }
        }

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="AutoDAO"/> com propriedades padrão.
        /// </summary>
        /// <param name="providerInfo">Informações do provedor de dados.</param>
        public AutoDAO(ProviderInfo providerInfo)
        {
            _identifier = providerInfo.Identifier;

            _connection = Core.DataObjects.CreateConnection(providerInfo);
            _command = Core.DataObjects.CreateCommand(_connection, providerInfo);

            if (providerInfo.LockTransaction == true)
            {
                _transaction = Core.DataObjects.CreateTransaction(_connection, providerInfo);
            }
        }

        /// <summary>
        /// Adiciona um conjunto de parâmetros de entrada automaticamente à instrução SQL.
        /// </summary>
        /// <typeparam name="T">Tipo de objeto a ser analisado.</typeparam>
        /// <param name="parameters">Objeto <typeparamref name="T"/> com os parâmetros de entrada da instrução SQL.</param>
        /// <exception cref="NotSupportedException">NotSupportedException</exception>
        /// <exception cref="TypeLoadException">TypeLoadException</exception>
        /// <exception cref="ArgumentNullException">ArgumentNullException</exception>
        /// <exception cref="ArgumentException">ArgumentException</exception>
        /// <exception cref="InvalidOperationException">InvalidOperationException</exception>
        /// <exception cref="TargetException">TargetException</exception>
        /// <exception cref="TargetParameterCountException">TargetParameterCountException</exception>
        /// <exception cref="MethodAccessException">MethodAccessException</exception>
        /// <exception cref="TargetInvocationException">TargetInvocationException</exception>
        /// <exception cref="FormatException">FormatException</exception>
        public void AddParameters<T>(T parameters) where T : class
        {
            try
            {
                Core.Engine.AddParameters(ref _command, _identifier, parameters);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Adiciona um conjunto de parâmetros de entrada à instrução SQL.
        /// </summary>
        /// <param name="parameters">Conjunto de parâmetros de entrada.</param>
        /// <exception cref="NotSupportedException">NotSupportedException</exception>
        public void AddInputParameters(object[][] parameters)
        {
            try
            {
                Core.Engine.AddInputParameters(ref _command, _identifier, parameters);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Adiciona um parâmetro de saída à instrução SQL.
        /// </summary>
        /// <param name="outputParameter">Nome do parâmetro de saída.</param>
        /// <exception cref="NotSupportedException">NotSupportedException</exception>
        public void AddOutputParameter(string outputParameter)
        {
            try
            {
                Core.Engine.AddOutputParameter(ref _command, _identifier, outputParameter);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Remove todos os parâmetros informados à instrução SQL.
        /// </summary>
        public void ClearParameters()
        {
            try
            {
                Core.Engine.ClearParameters(ref _command);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Remove um ou mais parâmetros informados à instrução SQL.
        /// </summary>
        /// <param name="parameters">Conjunto de parâmetros de entrada.</param>
        public void RemoveParameters(string[] parameters)
        {
            try
            {
                Core.Engine.RemoveParameters(ref _command, _identifier, parameters);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Executa uma instrução SQL e retorna o número de linhas afetadas.
        /// </summary>
        /// <param name="storedProcedure">Query/Stored procedure a ser executada.</param>
        /// <returns>O número de linhas afetadas.</returns>
        /// <exception cref="InvalidOperationException">InvalidOperationException</exception>
        public int ExecuteNonQuery(string storedProcedure)
        {
            try
            {
                int result = Core.Engine.ExecuteNonQuery(ref _connection, ref _command, ref _transaction, storedProcedure);
                ManageTransaction(TransactionBehaviour.CommitTransaction);
                return result;
            }
            catch
            {
                ManageTransaction(TransactionBehaviour.RollbackTransaction);
                throw;
            }
        }

        /// <summary>
        /// Executa uma consulta e retorna a primeira linha da primeira coluna do conjunto de resultados retornado pela consulta.
        /// </summary>
        /// <param name="storedProcedure">Query/Stored procedure a ser executada.</param>
        /// <returns>A primeira linha da primeira coluna do conjunto de resultados.</returns>
        public object ExecuteScalar(string storedProcedure)
        {
            try
            {
                object result = Core.Engine.ExecuteScalar(ref _connection, ref _command, ref _transaction, storedProcedure);
                ManageTransaction(TransactionBehaviour.CommitTransaction);
                return result;
            }
            catch
            {
                ManageTransaction(TransactionBehaviour.RollbackTransaction);
                throw;
            }
        }

        /// <summary>
        /// Executa um comando em uma fonte de dados e retorna um objeto <typeparamref name="T"/> preenchido.
        /// </summary>
        /// <typeparam name="T">Tipo de objeto a ser preenchido.</typeparam>
        /// <param name="storedProcedure">Query/Stored procedure a ser executada.</param>
        /// <returns>Um objeto <typeparamref name="T"/> preenchido.</returns>
        public T Query<T>(string storedProcedure) where T : new()
        {
            try
            {
                T result = Core.Engine.GetEntity<T>(ref _connection, ref _command, ref _transaction, storedProcedure);
                ManageTransaction(TransactionBehaviour.CommitTransaction);
                return result;
            }
            catch
            {
                ManageTransaction(TransactionBehaviour.RollbackTransaction);
                throw;
            }
        }

        /// <summary>
        /// Executa um comando em uma fonte de dados e retorna uma lista de <typeparamref name="T"/> preenchidos.
        /// </summary>
        /// <typeparam name="T">Tipo de objetos na lista.</typeparam>
        /// <param name="storedProcedure">Query/Stored procedure a ser executada.</param>
        /// <returns>Uma lista de <typeparamref name="T"/> preenchidos.</returns>
        public List<T> QueryList<T>(string storedProcedure) where T : new()
        {
            try
            {
                List<T> result = Core.Engine.GetEntityList<T>(ref _connection, ref _command, ref _transaction, storedProcedure);
                ManageTransaction(TransactionBehaviour.CommitTransaction);
                return result;
            }
            catch
            {
                ManageTransaction(TransactionBehaviour.RollbackTransaction);
                throw;
            }
        }

        #region Dispose

        /// <summary>
        /// Realiza as tarefas definidas pelo aplicativo associadas à liberação ou à redefinição de recursos não gerenciados.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Realiza as tarefas definidas pelo aplicativo associadas à liberação ou à redefinição de recursos não gerenciados.
        /// </summary>
        /// <param name="disposing">Indica se a liberação ou redefinição de recursos está ocorrendo ou não.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposed == false)
            {
                if (disposing == true)
                {
                    if (_connection.State == ConnectionState.Open)
                    {
                        _transaction?.Dispose();
                        _connection.Close();
                    }

                    _command.Dispose();
                    _connection.Dispose();
                }

                _identifier = string.Empty;
            }

            disposed = true;
        }

        /// <summary>
        /// Destrói a instância atual da classe <see cref="AutoDAO"/> com propriedades padrão.
        /// </summary>
        ~AutoDAO()
        {
            Dispose(false);
        }

        #endregion Dispose
    }
}