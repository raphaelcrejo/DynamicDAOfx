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
        private readonly IDbConnection _connection;
        private IDbCommand _command;

        private string _identifier = string.Empty;

        internal bool disposed = false;

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="AutoDAO"/> com propriedades padrão.
        /// </summary>
        /// <param name="providerInfo">Informações do provedor de dados.</param>
        public AutoDAO(ProviderInfo providerInfo)
        {
            _identifier = providerInfo.Identifier;

            _connection = Core.DataObjects.CreateConnection(providerInfo);
            _command = Core.DataObjects.CreateCommand(_connection, providerInfo);
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
                return Core.Engine.ExecuteNonQuery(_connection, _command, storedProcedure);
            }
            catch
            {
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
                return Core.Engine.ExecuteScalar(_connection, _command, storedProcedure);
            }
            catch
            {
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
                return Core.Engine.GetEntity<T>(_connection, _command, storedProcedure);
            }
            catch
            {
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
                return Core.Engine.GetEntityList<T>(_connection, _command, storedProcedure);
            }
            catch
            {
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