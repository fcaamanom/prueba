//using Sybase.Data.AseClient;
using AdoNetCore.AseClient;
using Newtonsoft.Json;
using System.Data;
using System.Diagnostics;

namespace ApiExtranjeros.Common
{
    public class SybaseDBUtil
    {
        private readonly Utilidades _utils;
        public SybaseDBUtil(Utilidades utilidades)
        {
            _utils = utilidades;
        }
        #region Parametros

        string msgError = "";
        string sistema = "WSTramitaYa";
        public const string PREFIJO_ERRORES = "ERROR ";

        AseConnection conexion;
        AseCommand comando;
        AseTransaction trans;

        #endregion

        #region Registro de Errores

        /// <summary>
        /// Se ponen los codigos ASCII [Error = E, Advertencia = A, Informacion = I, Exito = X]
        /// </summary>
        public enum tipoMensaje
        {
            Error = 69,
            Advertencia = 65,
            Informacion = 73,
            Exito = 88
        }

        /// <summary>
        /// Funcion auxiliar para escribir los eventos en la tabla GE_EVENTO, con un maximo de 1000 caracteres
        /// </summary>
        /// <param name="origen">El sistema que provoco el mensaje</param>
        /// <param name="usuario">El usuario que origino el evento</param>
        /// <param name="ip">La direccion IP del Usuario</param>
        /// <param name="tipo">Tipo de evento basado en el ENUM</param>
        /// <param name="mensaje">El mensaje a almacenar</param>
        public static void escribirEvent(string origen, string usuario, string ip, tipoMensaje tipo, string mensaje)
        {
            string cuerpo = "";
            string pendiente = "";
            //Si el mensaje esta vacio se acabo la recursión
            if (mensaje.Equals(string.Empty))
            {
                return;
            }
            else
            {
                if (mensaje.Length > 1000)
                {
                    cuerpo = mensaje.Substring(0, 1000);
                    pendiente = mensaje.Substring(1000);
                }
                else
                {
                    cuerpo = mensaje;
                    pendiente = string.Empty;
                }
            }
            string query = "INSERT GE_EVENTO (EVE_FECHA, EVE_SISTEMA, EVE_ORIGEN, EVE_USUARIO, " +
                                        "EVE_EQUIPO_IP, EVE_NIVEL, EVE_MENSAJE, EVE_ESTADO, EVE_USR_INSERT, EVE_DATE_INSERT) " +
                                        "VALUES (GETDATE(), 'TRAMITEYA', @origen, @usuario, " +
                                        "@ip, @nivel, @mensaje, 1, @usuario, getdate())";
            string strConex = new Utilidades().getConfig("connectionStrings:general");
            AseConnection conError = new AseConnection(strConex);
            AseParameter parError;
            try
            {
                conError.Open();
                AseCommand comandMsg = new AseCommand(query, conError);
                parError = new AseParameter("@origen", AseDbType.VarChar, 100);
                parError.Value = origen;
                comandMsg.Parameters.Add(parError);
                parError = new AseParameter("@usuario", AseDbType.VarChar, 30);
                parError.Value = usuario;
                comandMsg.Parameters.Add(parError);
                parError = new AseParameter("@ip", AseDbType.VarChar, 50);
                parError.Value = ip;
                comandMsg.Parameters.Add(parError);
                parError = new AseParameter("@nivel", AseDbType.Char, 1);
                parError.Value = Convert.ToChar(tipo);
                comandMsg.Parameters.Add(parError);
                parError = new AseParameter("@mensaje", AseDbType.VarChar, 4000);
                parError.Value = cuerpo;
                comandMsg.Parameters.Add(parError);
                comandMsg.ExecuteNonQuery();
                conError.Close();
            }
            catch (Exception ex)
            {
                string msgEx = "ERROR escribiendo LOG: " + mensaje + " [" + ex.Message + "]";
                escribirMsgLog(origen, msgEx, tipoMensaje.Error);
            }
            finally
            {
                if (conError.State == ConnectionState.Open)
                {
                    conError.Close();
                }
            }
            escribirEvent(origen, usuario, ip, tipo, pendiente);
        }

        /// <summary>
        /// Escribe a la BD los datos de mensaje de bitacora. 
        /// </summary>
        /// <param name="origen">Funcion del sistema</param>
        /// <param name="usuario">Usuario que ejecuto la accion</param>
        /// <param name="ip">IP desde donde se ejecuta la accion</param>
        /// <param name="tipo">Tipo de mensaje, basado en un enumerado</param>
        /// <param name="mensaje">Mensaje a ser escrito en la bitacora</param>
        /// <param name="adicional">Objeto con informacion adicional a ser almacenado.</param>
        public static void escribirMsgLog(string origen, string usuario, string ip, tipoMensaje tipo, string mensaje, object adicional)
        {
            string cuerpo = "";
            if (adicional != null)
            {
                string json = JsonConvert.SerializeObject(adicional);
                cuerpo = mensaje + " Object: [" + json + "]";
            }
            else
            {
                cuerpo = mensaje;
            }
            escribirEvent(origen, usuario, ip, tipo, cuerpo);
        }

        public static void escribirMsgLogLocal(string origen, string mensaje, tipoMensaje tip)
        {
            escribirMsgLog(origen, "system", "", tip, mensaje, null);
        }

        /// <summary>
        /// Escribe el error al Event View de la maquina en caso de error. 
        /// </summary>
        /// <param name="origen">Origen del error</param>
        /// <param name="mensaje">Mensaje a escribir</param>
        /// <param name="tip">Tipo de mensaje</param>
        public static void escribirMsgLog(string origen, string mensaje, tipoMensaje tip)
        {
            try
            {
                EventLogEntryType tipo;
                switch (tip)
                {
                    case tipoMensaje.Error:
                        tipo = EventLogEntryType.Error;
                        break;
                    case tipoMensaje.Advertencia:
                        tipo = EventLogEntryType.Warning;
                        break;
                    case tipoMensaje.Exito:
                        tipo = EventLogEntryType.SuccessAudit;
                        break;
                    default:
                        tipo = EventLogEntryType.Information;
                        break;
                }

                if (!EventLog.SourceExists(origen))
                {
                    EventLog.CreateEventSource(origen, "Application");
                }
                EventLog.WriteEntry(origen, mensaje, tipo);
            }
            catch (Exception)
            {
                //Error no controlado, contener y no se puede hacer mas.
            }
        }

        #endregion

        #region Conexion BD

        /// <summary>
        /// Función que retorna el error producido por otro método.
        /// </summary>
        /// <returns>Una hilera con el mensaje de error.</returns>
        public string getError()
        {
            return msgError;
        }

        /// <summary>
        /// Limpia los mensajes de error.
        /// </summary>
        public void clearError()
        {
            msgError = "";
        }

        /// <summary>
        /// Retorna la conexion a la base de datos.
        /// </summary>
        /// <returns>Un string con la conexion a base de datos.</returns>
        private string getConection(string conex)
        {
            try
            {
                //return ConfigurationManager.ConnectionStrings[conex].ConnectionString;
                return _utils.getConfig(conex);
            }
            catch (Exception)
            {
                return "Data Source=.;Initial Catalog=BD;Integrated Security=True";
            }
        }


        /// <summary>
        /// Funcion encargada de crear la conexion con el string de conexion
        /// </summary>
        /// <returns>Si logro o no realizar la labor.</returns>
        public bool createConection(string conex)
        {
            try
            {
                //conexion = new  Sybase.Data.AseClient.AseConnection(getConection(conex));
                conexion = new  AseConnection(getConection(conex));
                return true;
            }
            catch (Exception ex)
            {
                escribirMsgLogLocal(sistema, PREFIJO_ERRORES + " creando la conexion a la Base de datos: " + ex.Message, tipoMensaje.Error);
                msgError += PREFIJO_ERRORES + " creando la conexion a la Base de datos: " + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Abre la conexion para procesar.
        /// </summary>
        /// <returns>Si logro o no la operacion</returns>
        public bool openConection()
        {
            try
            {
                conexion.Open();
                return true;
            }
            catch (Exception ex)
            {
                escribirMsgLogLocal(sistema, PREFIJO_ERRORES + " abriendo la conexion a la Base de datos: " + ex.Message, tipoMensaje.Error);
                msgError += PREFIJO_ERRORES + " abriendo la conexion a la Base de datos: " + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Verifica si la conexion esta abierta.
        /// </summary>
        /// <returns>Si la conexion esta abierta</returns>
        public bool IsOpenConection()
        {
            return (conexion.State == ConnectionState.Open);
        }

        /// <summary>
        /// Verifica si la conexion esta cerrada.
        /// </summary>
        /// <returns>Si la conexion esta cerrada</returns>
        public bool IsCloseConnection()
        {
            return (conexion.State == ConnectionState.Closed);
        }

        /// <summary>
        /// Cierra la conexion activa de la clase
        /// </summary>
        /// <returns>Si logro o no la operacion</returns>
        public bool closeConection()
        {
            try
            {
                if (conexion.State == ConnectionState.Open)
                    conexion.Close();
                return true;
            }
            catch (Exception ex)
            {
                escribirMsgLogLocal(sistema, PREFIJO_ERRORES + " cerrando la conexion a la Base de datos: " + ex.Message, tipoMensaje.Error);
                msgError += PREFIJO_ERRORES + " cerrando la conexion a la Base de datos: " + ex.Message + Environment.NewLine;
                return false;
            }
        }

        /// <summary>
        /// Inicializa una transaccion
        /// </summary>
        /// <returns>Si logro o no iniciar la transaccion</returns>
        public bool beginTrans()
        {
            try
            {
                trans = conexion.BeginTransaction();
                return true;
            }
            catch (Exception ex)
            {
                escribirMsgLogLocal(sistema, PREFIJO_ERRORES + " iniciando la transaccion: " + ex.Message, tipoMensaje.Error);
                msgError += PREFIJO_ERRORES + " iniciando la transaccion: " + ex.Message + Environment.NewLine;
                return false;
            }
        }

        /// <summary>
        /// Hace el commit de la transaccion.
        /// </summary>
        /// <returns>Si logro realizar el commit, sino se busca el error.</returns>
        public bool commitTrans()
        {
            try
            {
                trans.Commit();
                return true;
            }
            catch (Exception ex)
            {
                escribirMsgLogLocal(sistema, PREFIJO_ERRORES + " haciendo el commit de la transaccion: " + ex.Message, tipoMensaje.Error);
                msgError += PREFIJO_ERRORES + " haciendo el commit de la transaccion: " + ex.Message + Environment.NewLine;
                return false;
            }
        }

        /// <summary>
        /// Hace rollback de la transaccion
        /// </summary>
        /// <returns>Si logro realizar el rollback, sino se busca el error.</returns>
        public bool rollbackTrans()
        {
            try
            {
                trans.Rollback();
                return true;
            }
            catch (Exception ex)
            {
                escribirMsgLogLocal(sistema, PREFIJO_ERRORES + " haciendo el rollback de la transaccion: " + ex.Message, tipoMensaje.Error);
                msgError += PREFIJO_ERRORES + " haciendo el rollback de la transaccion: " + ex.Message + Environment.NewLine;
                return false;
            }
        }

        public void limpiarComando()
        {
            comando.Parameters.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comando"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public bool setParameter(AseCommand comando, AseParameter[] param)
        {
            try
            {
                if (param == null) return true;
                foreach (AseParameter par in param)
                {
                    comando.Parameters.Add(par);
                }
                return true;
            }
            catch (Exception ex)
            {
                escribirMsgLogLocal(sistema, PREFIJO_ERRORES + " agregando parametros: " + ex.Message, tipoMensaje.Error);
                msgError += PREFIJO_ERRORES + " agregando parametros: " + ex.Message + Environment.NewLine;
                return false;
            }
        }

        /// <summary>
        /// Crea la conexion y la abre.
        /// </summary>
        /// <param name="withTrans">True inicia la transaccion, False sin transaccion</param>
        /// <returns>Si se logro todo en buen resultado</returns>
        public bool initConexion(string conex, bool withTrans)
        {
            clearError();
            bool resp = createConection(conex);
            if (!resp) return resp;
            resp = openConection();
            if (!resp) return resp;
            if (withTrans)
            {
                resp = beginTrans();
            }
            return resp;
        }

        /// <summary>
        /// Hacer un commit o rollback y luego cierra la conexion
        /// </summary>
        /// <param name="withCommit">True hace un commit, false hace un rollback</param>
        /// <returns>Si logro hacer la operacion o no.</returns>
        public bool closeConexionTrans(bool withCommit)
        {
            bool resp = true;
            clearError();
            if (IsOpenConection())
            {
                if (withCommit)
                {
                    resp = commitTrans();
                }
                else
                {
                    resp = rollbackTrans();
                }
                if (!resp) return resp;
                resp = closeConection();
                return resp;
            }
            else
            {
                msgError += PREFIJO_ERRORES + " se intenta cerrar la conexion y no esta abierta.";
                return false;
            }
        }

        /// <summary>
        /// Ejecuta un procedimiento con parametros que no devuelve nada.
        /// </summary>
        /// <param name="tipo">Tipo = 1 Procedimiento, Tipo = 0 Texto</param>
        /// <param name="procedimiento">Nombre del procedimiento almacenado</param>
        /// <param name="param">Coleccion de parametros</param>
        /// <returns>Si ejecuto el comando o no.</returns>
        public bool executeNoQuery(int tipo, string procedimiento, AseParameter[] param)
        {
            try
            {
                bool resp = true;
                comando = new AseCommand(procedimiento, conexion);
                if (tipo == 1)
                {
                    comando.CommandType = CommandType.StoredProcedure;
                }
                else
                {
                    comando.CommandType = CommandType.Text;
                }
                resp = setParameter(comando, param);
                if (!resp) return resp;
                int i = comando.ExecuteNonQuery();
                return resp;
            }
            catch (Exception ex)
            {
                escribirMsgLogLocal(sistema, PREFIJO_ERRORES + " ejecutando la consulta: " + procedimiento + ". " + ex.Message, tipoMensaje.Error);
                msgError += PREFIJO_ERRORES + " ejecutando la consulta: " + procedimiento + ". " + ex.Message + Environment.NewLine;
                return false;
            }
        }

        /// <summary>
        /// Ejecuta un procedimiento con parametros que no devuelve nada, usando una transaccion.
        /// </summary>
        /// <param name="tipo">Tipo = 1 Procedimiento, Tipo = 0 Texto</param>
        /// <param name="procedimiento">Nombre del procedimiento almacenado</param>
        /// <param name="param">Coleccion de parametros</param>
        /// <returns>Si ejecuto el comando o no.</returns>
        public bool executeNoQueryTrans(int tipo, string procedimiento, AseParameter[] param)
        {
            try
            {
                bool resp = true;
                comando = new AseCommand(procedimiento, conexion, trans);
                if (tipo == 1)
                {
                    comando.CommandType = CommandType.StoredProcedure;
                }
                else
                {
                    comando.CommandType = CommandType.Text;
                }
                resp = setParameter(comando, param);
                if (!resp) return resp;
                int i = comando.ExecuteNonQuery();
                return resp;
            }
            catch (Exception ex)
            {
                escribirMsgLogLocal(sistema, PREFIJO_ERRORES + " ejecutando la consulta: " + procedimiento + ". " + ex.Message, tipoMensaje.Error);
                msgError += PREFIJO_ERRORES + " ejecutando la consulta: " + procedimiento + ". " + ex.Message + Environment.NewLine;
                return false;
            }
        }

        /// <summary>
        /// Ejecuta un comando con parametros que devuelve un objeto.
        /// </summary>
        /// <param name="tipo">Tipo = 1 Procedimiento, Tipo = 0 Texto</param>
        /// <param name="procedimiento">El nombre del procedimiento almacenado.</param>
        /// <param name="param">Los parametros del comando</param>
        /// <returns>El objeto retornado por el comando, es null si dio error.</returns>
        public Object executeScalar(int tipo, string procedimiento, AseParameter[] param)
        {
            try
            {
                bool resp = true;
                comando = new AseCommand(procedimiento, conexion);
                if (tipo == 1)
                {
                    comando.CommandType = CommandType.StoredProcedure;
                }
                else
                {
                    comando.CommandType = CommandType.Text;
                }
                resp = setParameter(comando, param);
                if (!resp) return resp;
                Object fin = comando.ExecuteScalar();
                return fin;
            }
            catch (Exception ex)
            {
                escribirMsgLogLocal(sistema, PREFIJO_ERRORES + " ejecutando la consulta: " + procedimiento + ". " + ex.Message, tipoMensaje.Error);
                msgError += PREFIJO_ERRORES + " ejecutando la consulta: " + procedimiento + ". " + ex.Message + Environment.NewLine;
                return null;
            }
        }

        /// <summary>
        /// Ejecuta un comando con parametros que devuelve un objeto, utilizando una transaccion.
        /// </summary>
        /// <param name="tipo">Tipo = 1 Procedimiento, Tipo = 0 Texto</param>
        /// <param name="procedimiento">El nombre del procedimiento almacenado.</param>
        /// <param name="param">Los parametros del comando</param>
        /// <returns>El objeto retornado por el comando, es null si dio error.</returns>
        public Object executeScalarTrans(int tipo, string procedimiento, AseParameter[] param)
        {
            try
            {
                bool resp = true;
                comando = new AseCommand(procedimiento, conexion, trans);
                if (tipo == 1)
                {
                    comando.CommandType = CommandType.StoredProcedure;
                }
                else
                {
                    comando.CommandType = CommandType.Text;
                }
                resp = setParameter(comando, param);
                if (!resp) return resp;
                Object fin = comando.ExecuteScalar();
                return fin;
            }
            catch (Exception ex)
            {
                escribirMsgLogLocal(sistema, PREFIJO_ERRORES + " ejecutando la consulta: " + procedimiento + ". " + ex.Message, tipoMensaje.Error);
                msgError += PREFIJO_ERRORES + " ejecutando la consulta: " + procedimiento + ". " + ex.Message + Environment.NewLine;
                return null;
            }
        }

        /// <summary>
        /// Ejecuta un comando con parametros que devuelve una tabla.
        /// </summary>
        /// <param name="tipo">Tipo = 1 Procedimiento, Tipo = 0 Texto</param>
        /// <param name="procedimiento">El nombre del procedimiento almacenado.</param>
        /// <returns>La tabla que devolvio el comando, es nulo si dio error.</returns>
        public DataTable executeReader(int tipo, string procedimiento)
        {
            try
            {
                comando = new AseCommand(procedimiento, conexion);
                if (tipo == 1)
                {
                    comando.CommandType = CommandType.StoredProcedure;
                }
                else
                {
                    comando.CommandType = CommandType.Text;
                }
                AseDataReader read = comando.ExecuteReader();
                DataTable tabla = new DataTable();
                tabla.Load(read);
                return tabla;
            }
            catch (Exception ex)
            {
                escribirMsgLogLocal(sistema, PREFIJO_ERRORES + " ejecutando la consulta: " + procedimiento + ". " + ex.Message, tipoMensaje.Error);
                msgError += PREFIJO_ERRORES + " ejecutando la consulta: " + procedimiento + ". " + ex.Message + Environment.NewLine;
                return null;
            }
        }

        /// <summary>
        /// Ejecuta un comando con parametros que devuelve una tabla.
        /// </summary>
        /// <param name="tipo">Tipo = 1 Procedimiento, Tipo = 0 Texto</param>
        /// <param name="procedimiento">El nombre del procedimiento almacenado.</param>
        /// <param name="param">Los parametros del comando</param>
        /// <returns>La tabla que devolvio el comando, es nulo si dio error.</returns>
        public DataTable executeReader(int tipo, string procedimiento, AseParameter[] param)
        {
            try
            {
                bool resp = true;
                comando = new AseCommand(procedimiento, conexion);
                if (tipo == 1)
                {
                    comando.CommandType = CommandType.StoredProcedure;
                }
                else
                {
                    comando.CommandType = CommandType.Text;
                }
                resp = setParameter(comando, param);
                if (!resp) return null;
                AseDataReader read = comando.ExecuteReader();
                DataTable tabla = new DataTable();
                tabla.Load(read);
                return tabla;
            }
            catch (Exception ex)
            {
                escribirMsgLogLocal(sistema, PREFIJO_ERRORES + " ejecutando la consulta: " + procedimiento + ". " + ex.Message, tipoMensaje.Error);
                msgError += PREFIJO_ERRORES + " ejecutando la consulta: " + procedimiento + ". " + ex.Message + Environment.NewLine;
                return null;
            }
        }

        /// <summary>
        /// Ejecuta un comando con transaccion y con parametros que devuelve una tabla.
        /// </summary>
        /// <param name="tipo">Tipo = 1 Procedimiento, Tipo = 0 Texto</param>
        /// <param name="procedimiento">El nombre del procedimiento almacenado.</param>
        /// <param name="param">Los parametros del comando</param>
        /// <returns>La tabla que devolvio el comando, es nulo si dio error.</returns>
        public DataTable executeReaderTrans(int tipo, string procedimiento, AseParameter[] param)
        {
            try
            {
                bool resp = true;
                comando = new AseCommand(procedimiento, conexion, trans);
                if (tipo == 1)
                {
                    comando.CommandType = CommandType.StoredProcedure;
                }
                else
                {
                    comando.CommandType = CommandType.Text;
                }
                resp = setParameter(comando, param);
                if (!resp) return null;
                AseDataReader read = comando.ExecuteReader();
                DataTable tabla = new DataTable();
                tabla.Load(read);
                return tabla;
            }
            catch (Exception ex)
            {
                escribirMsgLogLocal(sistema, PREFIJO_ERRORES + " ejecutando la consulta: " + procedimiento + ". " + ex.Message, tipoMensaje.Error);
                msgError += PREFIJO_ERRORES + " ejecutando la consulta: " + procedimiento + ". " + ex.Message + Environment.NewLine;
                return null;
            }
        }

        #endregion
    }
}
