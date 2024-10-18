﻿
namespace ApiExtranjeros.Models.Queries
{
    public static class CatalogoSqlQueries
    {
        private static readonly Dictionary<string, string> _queries = new Dictionary<string, string>
        {
            //Catalogos
            {"getProvincia", "SELECT PROV_ID, PROV_DESCRIPCION FROM GE_PROVINCIA WHERE PROV_ESTADO = 1"},
            {"getCanton", "SELECT CAN_ID as ID, PROV_ID as IDProvincia, CAN_DESCRIPCION as Nombre FROM GE_CANTON WHERE CAN_ESTADO = 1 "},
            {"getDistrito", "SELECT DIS_ID as ID, CAN_ID as IDCanton, DIS_DESCRIPCION as Nombre FROM GE_DISTRITO WHERE DIS_ESTADO = 1"},
            {"getNacionalidad", "SELECT NAC_ID as ID, NAC_DESCRIPCION as Nombre FROM GE_NACIONALIDAD WHERE NAC_ESTADO = 1"},
            {"getOcupacion", "SELECT OCU_ID as ID, OCU_DESCRIPCION as Nombre FROM GE_OCUPACION WHERE OCU_ESTADO = 1"},
            {"getPuestoMigratorio", "SELECT PUE_ID as ID, PUE_DESCRIPCION as Nombre FROM GE_PUESTO_MIGRATORIO WHERE PUE_ESTADO = 1"},
            {"getEntidad", "SELECT ENT_ID as ID, ENT_NOMBRE as Nombre, NAC_ID_UBICACION as IDNacionalidad FROM GE_ENTIDAD WHERE ENT_ESTADO = 1"},
            {"getTipoIdentificacion", "SELECT DOC_ID as ID, DOC_DESCRIPCION as Nombre FROM GE_DOCUMENTO WHERE DOC_ESTADO = 1"},
            {"getCondicionMigratoria", "SELECT CON_MIG_ID as ID, CON_MIG_CONDICION as Nombre FROM EG_CONDICION_MIGRATORIA WHERE CON_MIG_ESTADO = 1"},
            {"getCondicionLaboral", "SELECT CON_LAB_ID as ID, CON_LAB_CONDICION as Nombre FROM EG_CONDICION_LABORAL WHERE CON_LAB_ESTADO = 1"},
            {"getTipoSolicitud", "SELECT TIP_SOL_ID as ID, TIP_SOL_NOMBRE as Nombre, TIP_SOL_DESCRIPCION as Descripcion FROM EX_TIPO_SOLICITUD WHERE TIP_SOL_ESTADO = 1"},
            {"getTipoResolucion", "SELECT TIP_RESOL_ID as ID, TIP_RESOL_DESCRIPCION as Nombre FROM VR_TIPO_RESOLUCION WHERE TIP_RESOL_ESTADO = 1"},
            {"getRequisito", "SELECT DOC_ID as ID, DOC_NOMBRE as Nombre, DOC_TIPO_REQUISITO as TipoRequisito, DOC_PLAZO_DIAS as PlazoDias FROM EX_DOCUMENTO WHERE DOC_ESTADO = 1"},
            {"getEmpresa", "SELECT E.EMP_ID as ID, E.EMP_CEDULA_JURIDICA as CedulaJuridica, E.EMP_NOMBRE as Nombre, E.EMP_ALIAS as Alias, E.EMP_GIRO_COMERCIAL as GiroComercial, E.EMP_ID_REPRESENTANTE_LEGAL as IdRepresentanteLegal, E.EMP_REPRESENTANTE_LEGAL as NombreRepresentanteLegal, C.EMP_CAT_TIPO as Categoria, C.EMP_CAT_FECHA_RENOVACION as FechaRenovacion      FROM EX_EMPRESA E                   JOIN EX_EMPRESA_CATEGORIA_ACTUAL C ON E.EMP_ID = C.EMP_ID                     WHERE E.EMP_ESTADO = 1 AND C.EMP_ESTADO = 1                      AND C.EMP_CAT_FECHA_RENOVACION > GETDATE()"},
            {"getPronunciamientoVisas", "SELECT PRO_RES_CODIGO_RESOLUCION as Resolucion, PRO_RES_CODIGO_ESTUDIO as Estudio, PRO_RES_DESCRIPCION as Descripcion FROM GE_PRONUNCIAMIENTO_RESOLUCION WHERE PREF_ID='VR' AND PRO_RES_ESTADO=1"},
            {"getPronunciamientoExtranjeria", "SELECT PRO_RES_CODIGO_RESOLUCION as Resolucion,PRO_RES_CODIGO_ESTUDIO as Estudio,PRO_RES_DESCRIPCION as Descripcion FROM GE_PRONUNCIAMIENTO_RESOLUCION WHERE PREF_ID='EX' AND PRO_RES_ESTADO=1"},
            {"getPlantillas", "SELECT  P.TIP_SOL_ID as Cod_Tipo_Solicitud, T.TIP_SOL_NOMBRE as Tipo_Solicitud_Nombre,P.CON_MIG_ID as Cod_Condicion_Migratoria, M.CON_MIG_CONDICION as Migracion_Condicion_Nombre,P.CON_LAB_ID as Cod_Condicion_Laboratorio, L.CON_LAB_CONDICION as Lab_Condicion_Nombre, P.DOC_ID as Cod_Documento, D.DOC_NOMBRE as Doc_Nombre, P.PLA_REQ_NATURALEZA as Naturaleza_Tramite, P.PLA_REQ_REQUERIDO as Pla_Requerida    FROM EX_PLANTILLA_REQUISITO P , EX_TIPO_SOLICITUD T, EG_CONDICION_MIGRATORIA M, EG_CONDICION_LABORAL L, EX_DOCUMENTO D       WHERE    P.TIP_SOL_ID = T.TIP_SOL_ID AND   P.CON_MIG_ID = M.CON_MIG_ID  AND     P.CON_LAB_ID = L.CON_LAB_ID AND   P.DOC_ID = D.DOC_ID AND   P.PLA_REQ_ESTADO = 1   ORDER BY P.TIP_SOL_ID"},
            //Busquedas
            {"BuscarEmpresa", "SELECT  E.EMP_ID AS IdConsecutivoEmpresa, E.EMP_CEDULA_JURIDICA AS CedulaJuridica, E.EMP_NOMBRE AS NombreEmpresa, E.EMP_ALIAS AS AliasEmpresa, E.EMP_GIRO_COMERCIAL AS GiroComercial, E.EMP_ID_REPRESENTANTE_LEGAL AS IdRepresentanteLegal, E.EMP_REPRESENTANTE_LEGAL AS NombreRepresentante, C.EMP_CAT_TIPO AS CategoriaEmpresa, C.EMP_CAT_FECHA_RENOVACION AS FechaRenovacion         FROM EX_EMPRESA E, EX_EMPRESA_CATEGORIA_ACTUAL C    WHERE E.EMP_ESTADO = 1 AND C.EMP_ESTADO = 1 AND E.EMP_ID = C.EMP_ID    "},
            {"BuscarDocumentoUnico", "SELECT EG_CALIDAD.CAL_NOMBRE AS Nombre, EG_DOCUMENTO_UNICO.DOC_NUMERO_DOCUMENTO, EG_CALIDAD.CAL_PRIMER_APELLIDO AS PrimerApellido, EG_CALIDAD.CAL_SEGUNDO_APELLIDO AS SegundoApellido,  EG_CALIDAD.CAL_FECHA_NACIMIENTO AS FechaNacimiento, EG_CALIDAD.NAC_ID AS Nacionalidad, EG_CONDICION_MIGRATORIA.CON_MIG_CONDICION AS CondicionMigratoria,  EG_CONDICION_LABORAL.CON_LAB_CONDICION AS CondicionLaboral, EG_DOCUMENTO_UNICO.DOC_FECHA_EMISION AS FechaEmision, EG_DOCUMENTO_UNICO.DOC_FECHA_RENOVACION AS FechaRenovacion      FROM EG_DOCUMENTO_UNICO, EG_CALIDAD, EG_CONDICION_MIGRATORIA,EG_CONDICION_LABORAL   WHERE EG_DOCUMENTO_UNICO.CON_MIG_ID = EG_CONDICION_MIGRATORIA.CON_MIG_ID   AND EG_DOCUMENTO_UNICO.CON_LAB_ID=EG_CONDICION_LABORAL.CON_LAB_ID   AND EG_DOCUMENTO_UNICO.PUE_ID_CALIDAD=EG_CALIDAD.PUE_ID   AND EG_DOCUMENTO_UNICO.CAL_ID_CALIDAD=EG_CALIDAD.CAL_ID_CALIDAD     AND LTRIM(RTRIM(EG_DOCUMENTO_UNICO.DOC_NUMERO_DOCUMENTO)) = LTRIM(RTRIM(@NumeroDocUnico))     AND EG_DOCUMENTO_UNICO.DOC_UNI_ESTADO=1"},
            {"BuscarDeposito", "SELECT BM.MOV_TIPO AS tipo, BM.MOV_NUMERO AS numeroDeposito, BM.MOV_CUENTA_BANCARIA AS cuentaBancaria, BM.MOV_IDENTIFICACION AS identificacion, BM.MOV_NOMBRE AS Nombre, BM.MOV_OBSERVACIONES AS Observaciones, BM.MOV_MONTO AS monto,BM.TRAMITE_ID AS tramite, BM.MOV_FECHA AS fechadeposito, BM.MOV_UTILIZADO AS movutilizado, BM.MOV_ESTADO AS estado     FROM BA_MOVIMIENTOS as BM   WHERE LTRIM(RTRIM(BM.MOV_NUMERO)) = LTRIM(RTRIM(@numeroDeposito))   AND BM.MOV_ESTADO = 1  AND cast(BM.MOV_FECHA as date) = cast(@fechadeposito as date)    ORDER BY MOV_DATE_INSERT DESC"},
            {"ObtenerPlantillaRequisito","SELECT P.tip_sol_id AS IdTipoSolicitud, T.tip_sol_nombre AS TipoSolicitud, P.con_mig_id AS IdCondicionMigratoria, M.con_mig_condicion AS CondicionMigratoria, P.con_lab_id AS IdCondicionLaboral, L.con_lab_condicion AS CondicionLaboral, P.doc_id AS IdDocumento, D.doc_nombre AS Documento, P.pla_req_naturaleza AS NaturalezaTramite, P.pla_req_requerido AS RequisitoRequerido FROM ex_plantilla_requisito P, ex_tipo_solicitud T, eg_condicion_migratoria M, eg_condicion_laboral L, ex_documento D WHERE P.tip_sol_id = @IdTipoSolicitud AND P.con_mig_id = @IdCondicionMigratoria AND P.con_lab_id = @IdCondicionLaboral AND P.pla_req_estado = 1 AND P.pla_req_naturaleza = UPPER(@NaturalezaTramite) AND P.pla_req_requerido IN ('S', 'O') AND P.tip_sol_id = T.tip_sol_id AND P.con_mig_id = M.con_mig_id AND P.con_lab_id = L.con_lab_id AND P.doc_id = D.doc_id" },
            {"BuscarDGME/Full","SELECT ISNULL(E.EXP_NUM_EXPEDIENTE, 0) as NumeroExpediente, ISNULL(E.PUE_ID_EXPEDIENTE,0) as PuestoMigratorio,ISNULL(EXP_FECHA_EXPEDIENTE,getdate()) as FechaExpediente,ISNULL(EXP_ESTADO_EXPEDIENTE, '0') as EstadoExpediente, C.PUE_ID as PuestoCalidad, C.CAL_ID_CALIDAD as Calidad, C.DOC_ID as DocumentoUnico, C.CAL_NUMERO_DOCUMENTO as Identificacion, C.CAL_FECHA_NACIMIENTO as FechaNacimiento, C.NAC_ID as Nacionalidad,   C.CAL_NOMBRE as Nombre, C.CAL_PRIMER_APELLIDO as PrimerApellido, C.CAL_SEGUNDO_APELLIDO  as SegundoApellido, 0 as CondicionMigratoria, 0 as CondicionLaboral  FROM(SELECT * FROM EG_CALIDAD C WHERE CAST(CAL_FECHA_NACIMIENTO AS DATE) = CAST(@FechaNacimiento AS DATE) AND NAC_ID = @Nacionalidad  AND LTRIM(RTRIM(CAL_NOMBRE)) =  LTRIM(RTRIM(@Nombre)) AND LTRIM(RTRIM(CAL_PRIMER_APELLIDO)) = LTRIM(RTRIM(@PrimerApellido)) AND LTRIM(RTRIM(CAL_SEGUNDO_APELLIDO)) = LTRIM(RTRIM(@SegundoApellido))) AS C   LEFT OUTER JOIN EX_EXPEDIENTE E   ON E.PUE_ID_CALIDAD = C.PUE_ID AND E.CAL_ID_CALIDAD = C.CAL_ID_CALIDAD AND DOC_ID = @TipoIdentificacion AND E.EXP_NUM_EXPEDIENTE = cast(@NumeroExpediente as int) AND LTRIM(RTRIM(CAL_NUMERO_DOCUMENTO)) = LTRIM(RTRIM(@Identificacion)) AND E.PUE_ID_EXPEDIENTE = @PuestoMigratorio " },
            {"BuscarDGME/Expediente","SELECT E.EXP_NUM_EXPEDIENTE as NumeroExpediente,E.PUE_ID_EXPEDIENTE as PuestoMigratorio, EXP_FECHA_EXPEDIENTE as FechaExpediente,EXP_ESTADO_EXPEDIENTE as EstadoExpediente, C.PUE_ID as PuestoCalidad, C.CAL_ID_CALIDAD as Calidad,C.DOC_ID as DocumentoUnico,C.CAL_NUMERO_DOCUMENTO as Identificacion,C.CAL_FECHA_NACIMIENTO as FechaNacimiento, C.NAC_ID as Nacionalidad,  C.CAL_NOMBRE, C.CAL_PRIMER_APELLIDO as PrimerApellido, C.CAL_SEGUNDO_APELLIDO  as SegundoApellido, 0 as CondicionMigratoria, 0 as CondicionLaboral FROM EX_EXPEDIENTE E, EG_CALIDAD C  WHERE E.EXP_ESTADO = 1  AND E.EXP_NUM_EXPEDIENTE = cast(@NumeroExpediente as int) AND E.PUE_ID_CALIDAD = C.PUE_ID AND E.CAL_ID_CALIDAD = C.CAL_ID_CALIDAD " },
            {"BuscarDGME/Identificacion","SELECT ISNULL(E.EXP_NUM_EXPEDIENTE , 0) as NumeroExpediente, ISNULL(E.PUE_ID_EXPEDIENTE,0) as PuestoMigratorio, ISNULL(EXP_FECHA_EXPEDIENTE,getdate()) as FechaExpediente,  ISNULL(EXP_ESTADO_EXPEDIENTE, '0') as EstadoExpediente, C.PUE_ID as PuestoCalidad, C.CAL_ID_CALIDAD as Calidad,  C.DOC_ID as DocumentoUnico, C.CAL_NUMERO_DOCUMENTO as Identificacion, C.CAL_FECHA_NACIMIENTO as FechaNacimiento, C.NAC_ID as Nacionalidad,  C.CAL_NOMBRE as Nombre, C.CAL_PRIMER_APELLIDO as PrimerApellido, C.CAL_SEGUNDO_APELLIDO  as SegundoApellido, 0 as CondicionMigratoria, 0 as CondicionLaboral  FROM(SELECT * FROM EG_CALIDAD C WHERE DOC_ID = @TipoIdentificacion  AND LTRIM(RTRIM(CAL_NUMERO_DOCUMENTO)) = LTRIM(RTRIM(@Identificacion)) ) AS C  LEFT OUTER JOIN EX_EXPEDIENTE E  ON E.PUE_ID_CALIDAD = C.PUE_ID AND E.CAL_ID_CALIDAD = C.CAL_ID_CALIDAD " },
            {"BuscarDGME/ApellidoYNombre","SELECT ISNULL(E.EXP_NUM_EXPEDIENTE, 0) as NumeroExpediente, ISNULL(E.PUE_ID_EXPEDIENTE,0) as PuestoMigratorio,ISNULL(EXP_FECHA_EXPEDIENTE,getdate()) as FechaExpediente,ISNULL(EXP_ESTADO_EXPEDIENTE, '0') as EstadoExpediente, C.PUE_ID as PuestoCalidad, C.CAL_ID_CALIDAD as Calidad, C.DOC_ID as DocumentoUnico, C.CAL_NUMERO_DOCUMENTO as Identificacion, C.CAL_FECHA_NACIMIENTO as FechaNacimiento, C.NAC_ID as Nacionalidad,   C.CAL_NOMBRE as Nombre, C.CAL_PRIMER_APELLIDO as PrimerApellido, C.CAL_SEGUNDO_APELLIDO  as SegundoApellido, 0 as CondicionMigratoria, 0 as CondicionLaboral   FROM(SELECT * FROM EG_CALIDAD C WHERE NAC_ID = @Nacionalidad  AND LTRIM(RTRIM(CAL_NOMBRE)) =  LTRIM(RTRIM(@Nombre)) AND LTRIM(RTRIM(CAL_PRIMER_APELLIDO)) = LTRIM(RTRIM(@PrimerApellido)) AND LTRIM(RTRIM(ISNULL(CAL_SEGUNDO_APELLIDO,''))) = LTRIM(RTRIM(@SegundoApellido))) AS C   LEFT OUTER JOIN EX_EXPEDIENTE E   ON E.PUE_ID_CALIDAD = C.PUE_ID AND E.CAL_ID_CALIDAD = C.CAL_ID_CALIDAD " },
            {"BuscarDGME/ApellidoYNombreLike","SELECT ISNULL(E.EXP_NUM_EXPEDIENTE, 0) as NumeroExpediente, ISNULL(E.PUE_ID_EXPEDIENTE,0) as PuestoMigratorio,ISNULL(EXP_FECHA_EXPEDIENTE,getdate()) as FechaExpediente,ISNULL(EXP_ESTADO_EXPEDIENTE, '0') as EstadoExpediente, C.PUE_ID as PuestoCalidad, C.CAL_ID_CALIDAD as Calidad, C.DOC_ID as DocumentoUnico, C.CAL_NUMERO_DOCUMENTO as Identificacion, C.CAL_FECHA_NACIMIENTO as FechaNacimiento, C.NAC_ID as Nacionalidad,   C.CAL_NOMBRE as Nombre, C.CAL_PRIMER_APELLIDO as PrimerApellido, C.CAL_SEGUNDO_APELLIDO  as SegundoApellido, 0 as CondicionMigratoria, 0 as CondicionLaboral   FROM(SELECT * FROM EG_CALIDAD C WHERE  NAC_ID = @Nacionalidad  AND LTRIM(RTRIM(CAL_NOMBRE)) LIKE  LTRIM(RTRIM(@Nombre)) AND LTRIM(RTRIM(CAL_PRIMER_APELLIDO)) LIKE LTRIM(RTRIM(@PrimerApellido)) AND LTRIM(RTRIM(ISNULL(CAL_SEGUNDO_APELLIDO,''))) LIKE LTRIM(RTRIM(@SegundoApellido))) AS C   LEFT OUTER JOIN EX_EXPEDIENTE E   ON E.PUE_ID_CALIDAD = C.PUE_ID AND E.CAL_ID_CALIDAD = C.CAL_ID_CALIDAD " },
            {"BuscarDGME/NuevoExpediente","SELECT ISNULL(E.EXP_NUM_EXPEDIENTE, 0) as NumeroExpediente, ISNULL(E.PUE_ID_EXPEDIENTE,0) as PuestoMigratorio,ISNULL(EXP_FECHA_EXPEDIENTE,getdate()) as FechaExpediente,ISNULL(EXP_ESTADO_EXPEDIENTE, '0') as EstadoExpediente, C.PUE_ID as PuestoCalidad, C.CAL_ID_CALIDAD as Calidad, C.DOC_ID as DocumentoUnico, C.CAL_NUMERO_DOCUMENTO as Identificacion, C.CAL_FECHA_NACIMIENTO as FechaNacimiento, C.NAC_ID as Nacionalidad,   C.CAL_NOMBRE as Nombre, C.CAL_PRIMER_APELLIDO as PrimerApellido, C.CAL_SEGUNDO_APELLIDO  as SegundoApellido, 0 as CondicionMigratoria, 0 as CondicionLaboral   FROM(SELECT * FROM EG_CALIDAD C WHERE  NAC_ID = @Nacionalidad AND CAST(C.CAL_FECHA_NACIMIENTO AS DATE) = CAST(@FechaNacimiento AS DATE) AND LTRIM(RTRIM(CAL_NOMBRE)) LIKE  LTRIM(RTRIM(@Nombre)) AND LTRIM(RTRIM(CAL_PRIMER_APELLIDO)) LIKE LTRIM(RTRIM(@PrimerApellido)) AND LTRIM(RTRIM(ISNULL(CAL_SEGUNDO_APELLIDO,''))) LIKE LTRIM(RTRIM(@SegundoApellido)) ) AS C   LEFT OUTER JOIN EX_EXPEDIENTE E   ON E.PUE_ID_CALIDAD = C.PUE_ID AND E.CAL_ID_CALIDAD = C.CAL_ID_CALIDAD" },
            {"BuscarCalidadXId", "SELECT CAL_ID_CALIDAD FROM EG_CALIDAD WHERE CAL_ID_CALIDAD = @CalidadId;"},
            {"BuscarSolicitud", "SELECT SOL_NUM_SOLICITUD FROM EX_SOLICITUD WHERE SOL_NUM_SOLICITUD=@NumeroSolicitud AND SOL_ESTADO = 1;"},




    };

        public static string GetQuery(string key)
        {
            if (_queries.TryGetValue(key, out string query))
            {
                return query;
            }
            throw new KeyNotFoundException($"La consulta '{key}' no se encontró en el diccionario de SQL.");
        }

    }
}
