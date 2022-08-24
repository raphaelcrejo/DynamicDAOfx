using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace DynamicDAO.Core
{
    /// <summary>
    /// Define os métodos adição e remoção de parâmetros, execução de comandos em uma conexão à uma fonte de dados e devolução de objetos preenchidos.
    /// </summary>
    internal static class Engine
    {
        /// <summary>
        /// Preenche um objeto <typeparamref name="T"/> com o retorno da query/stored procedure executada.
        /// </summary>
        /// <typeparam name="T">Tipo do objeto a ser preenchido.</typeparam>
        /// <param name="reader">Fluxo somente encaminhamento de conjuntos de resultados obtidos.</param>
        /// <returns>Um objeto <typeparamref name="T"/> preenchido.</returns>
        /// <exception cref="TypeLoadException">TypeLoadException</exception>
        /// <exception cref="ArgumentNullException">ArgumentNullException</exception>
        /// <exception cref="ArgumentException">ArgumentException</exception>
        /// <exception cref="InvalidOperationException">InvalidOperationException</exception>
        /// <exception cref="IndexOutOfRangeException">IndexOutOfRangeException</exception>
        /// <exception cref="AmbiguousMatchException">AmbiguousMatchException</exception>
        /// <exception cref="InvalidCastException">InvalidCastException</exception>
        /// <exception cref="FormatException">FormatException</exception>
        /// <exception cref="OverflowException">OverflowException</exception>
        /// <exception cref="TargetException">TargetException</exception>
        /// <exception cref="TargetParameterCountException">TargetParameterCountException</exception>
        /// <exception cref="MethodAccessException">MethodAccessException</exception>
        /// <exception cref="TargetInvocationException">TargetInvocationException</exception>
        internal static T ConvertedEntity<T>(this IDataReader reader) where T : new()
        {
            string colName;
            Type t = typeof(T);
            T returnObject = new T();
            Dictionary<string, string> dicFields = Mapping.Mapper.GetFields<T>();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                colName = reader.GetName(i);

                PropertyInfo pInfo = t.GetProperty(dicFields[colName], BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (pInfo != null)
                {
                    object val = reader[colName];
                    bool isNullable = Nullable.GetUnderlyingType(pInfo.PropertyType) != null;

                    if (isNullable)
                    {
                        if (val is DBNull)
                        {
                            val = null;
                        }
                        else
                        {
                            val = Convert.ChangeType(val, Nullable.GetUnderlyingType(pInfo.PropertyType));
                        }
                    }
                    else
                    {
                        val = Convert.ChangeType(val, pInfo.PropertyType);
                    }

                    pInfo.SetValue(returnObject, val, null);
                }
            }

            return returnObject;
        }

        /// <summary>
        /// Adiciona um conjunto de parâmetros de entrada automaticamente à instrução SQL.
        /// </summary>
        /// <typeparam name="T">Tipo de objeto a ser analisado.</typeparam>
        /// <param name="command">Instrução SQL a ser executada.</param>
        /// <param name="identifier">Identificador de parâmetros.</param>
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
        internal static void AddParameters<T>(ref IDbCommand command, string identifier, T parameters) where T : class
        {
            try
            {
                command.Parameters.Clear();

                Dictionary<string, object[]> dicParameters = Mapping.Mapper.GetParameters<T>();

                foreach (var property in parameters.GetType().GetProperties())
                {
                    object value = property.GetValue(parameters, null);
                    var parameter = command.CreateParameter();

                    if (dicParameters.ContainsKey(property.Name) == false)
                    {
                        continue;
                    }

                    parameter.ParameterName = string.Format("{0}{1}", identifier, dicParameters[property.Name][0]);
                    parameter.Value = value ?? DBNull.Value;
                    parameter.Direction = (ParameterDirection)dicParameters[property.Name][1];

                    command.Parameters.Add(parameter);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Adiciona um conjunto de parâmetros de entrada à instrução SQL.
        /// </summary>
        /// <param name="command">Instrução SQL a ser executada.</param>
        /// <param name="identifier">Identificador de parâmetros.</param>
        /// <param name="parameters">Conjunto de parâmetros de entrada.</param>
        /// <exception cref="NotSupportedException">NotSupportedException</exception>
        internal static void AddInputParameters(ref IDbCommand command, string identifier, object[][] parameters)
        {
            try
            {
                command.Parameters.Clear();

                foreach (object[] param in parameters)
                {
                    var parameter = command.CreateParameter();

                    parameter.ParameterName = string.Format("{0}{1}", identifier, param[0].ToString());
                    parameter.Value = param[1] ?? DBNull.Value;
                    parameter.Direction = ParameterDirection.Input;

                    command.Parameters.Add(parameter);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Adiciona um parâmetro de saída à instrução SQL.
        /// </summary>
        /// <param name="command">Instrução SQL a ser executada.</param>
        /// <param name="identifier">Identificador de parâmetros.</param>
        /// <param name="outputParameter">Nome do parâmetro de saída.</param>
        /// <param name="clearParameters">Indica se os parâmetros já existentes an instrução SQL devem ser removidos ou não.</param>
        /// <exception cref="NotSupportedException">NotSupportedException</exception>
        internal static void AddOutputParameter(ref IDbCommand command, string identifier, string outputParameter, bool clearParameters = false)
        {
            try
            {
                if (clearParameters == true)
                {
                    command.Parameters.Clear();
                }

                var parameter = command.CreateParameter();

                parameter.ParameterName = string.Format("{0}{1}", identifier, outputParameter);
                parameter.Direction = ParameterDirection.Output;

                command.Parameters.Add(parameter);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Remove todos os parâmetros informados à instrução SQL.
        /// </summary>
        /// <param name="command">Instrução SQL a ser executada.</param>
        internal static void ClearParameters(ref IDbCommand command)
        {
            try
            {
                command.Parameters.Clear();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Remove um ou mais parâmetros informados à instrução SQL.
        /// </summary>
        /// <param name="command">Instrução SQL a ser executada.</param>
        /// <param name="identifier">Identificador de parâmetros.</param>
        /// <param name="parameters">Conjunto de parâmetros de entrada.</param>
        internal static void RemoveParameters(ref IDbCommand command, string identifier, string[] parameters)
        {
            try
            {
                foreach (string param in parameters)
                {
                    command.Parameters.RemoveAt(string.Format("{0}{1}", identifier, param));
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Executa uma instrução SQL no objeto <paramref name="connection"/> e retorna o número de linhas afetadas.
        /// </summary>
        /// <param name="connection">Conexão com a fonte de dados</param>
        /// <param name="command">Instrução SQL a ser executada.</param>
        /// <param name="storedProcedure">Query/Stored procedure a ser executada.</param>
        /// <returns>O número de linhas afetadas.</returns>
        /// <exception cref="InvalidOperationException">InvalidOperationException</exception>
        internal static int ExecuteNonQuery(IDbConnection connection, IDbCommand command, string storedProcedure)
        {
            try
            {
                command.CommandText = storedProcedure;

                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                return command.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Executa uma consulta e retorna a primeira linha da primeira coluna do conjunto de resultados retornado pela consulta.
        /// </summary>
        /// <param name="connection">Conexão com a fonte de dados</param>
        /// <param name="command">Instrução SQL a ser executada.</param>
        /// <param name="storedProcedure">Query/Stored procedure a ser executada.</param>
        /// <returns>A primeira linha da primeira coluna do conjunto de resultados.</returns>
        internal static object ExecuteScalar(IDbConnection connection, IDbCommand command, string storedProcedure)
        {
            try
            {
                command.CommandText = storedProcedure;

                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                return command.ExecuteScalar();
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
        /// <param name="connection">Conexão com a fonte de dados</param>
        /// <param name="command">Instrução SQL a ser executada.</param>
        /// <param name="storedProcedure">Query/Stored procedure a ser executada.</param>
        /// <returns>Um objeto <typeparamref name="T"/> preenchido.</returns>
        internal static T GetEntity<T>(IDbConnection connection, IDbCommand command, string storedProcedure) where T : new()
        {
            T returnObject = new T();

            try
            {
                command.CommandText = storedProcedure;

                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        returnObject = reader.ConvertedEntity<T>();
                    }
                }

                return returnObject;
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
        /// <param name="connection">Conexão com a fonte de dados</param>
        /// <param name="command">Instrução SQL a ser executada.</param>
        /// <param name="storedProcedure">Query/Stored procedure a ser executada.</param>
        /// <returns>Uma lista de <typeparamref name="T"/> preenchidos.</returns>
        internal static List<T> GetEntityList<T>(IDbConnection connection, IDbCommand command, string storedProcedure) where T : new()
        {
            List<T> returnObject = new List<T>();

            try
            {
                command.CommandText = storedProcedure;

                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        T addObject = reader.ConvertedEntity<T>();
                        returnObject.Add(addObject);
                    }
                }

                return returnObject;
            }
            catch
            {
                throw;
            }
        }
    }
}