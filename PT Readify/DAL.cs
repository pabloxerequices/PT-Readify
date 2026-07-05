using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Runtime.Remoting.Contexts;
using System.Windows.Forms;

namespace DataAccessLayer
{
    public class DAL
    {
        private SqlConnection _SqlConn;
        private SqlCommand _SqlCommand;
        private SqlDataReader _SqlReader;

        public DAL()
        {
            // Define a pasta de execução como o DataDirectory
            AppDomain.CurrentDomain.SetData("DataDirectory", AppDomain.CurrentDomain.BaseDirectory);

            // String direta e simples apontando para o ficheiro mdf
            string connString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Base_Dados.mdf;Integrated Security=True;Connect Timeout=30;MultipleActiveResultSets=True";

            _SqlConn = new SqlConnection(connString);
        }

        private void abrirLigacao()
        {
            try
            {
                _SqlConn.Open();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void fecharLigacao()
        {
            try
            {
                if (_SqlConn.State == ConnectionState.Open)
                {
                    _SqlConn.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void associarComando(String sqlCmd)
        {
            _SqlCommand = new SqlCommand(sqlCmd, _SqlConn);
        }

        public DataTable executarStoredProcReader(String sqlCmd, SqlParameter[] sqlParams)
        {
            DataTable returnTable = new DataTable("returnTable");
            associarComando(sqlCmd);
            _SqlCommand.CommandType = CommandType.StoredProcedure;

            if (sqlParams != null)
                _SqlCommand.Parameters.AddRange(sqlParams);

            try
            {
                abrirLigacao();

                if (_SqlConn.State == ConnectionState.Open)
                {
                    using (_SqlReader = _SqlCommand.ExecuteReader())
                    {
                        returnTable.Load(_SqlReader);
                    }
                }
            }
            finally
            {
                fecharLigacao();
                _SqlCommand.Parameters.Clear();
            }

            return returnTable;
        }

        //Stored Procedure sem parâmetros
        public DataTable executarStoredProcReader(String sqlCmd)
        {
            return this.executarStoredProcReader(sqlCmd, null);
        }

        public int executarStoredProcNonQuery(String sqlCmd, SqlParameter[] sqlParams)
        {
            int retorno = -1;
            associarComando(sqlCmd);
            _SqlCommand.CommandType = CommandType.StoredProcedure;

            if (sqlParams != null)
                _SqlCommand.Parameters.AddRange(sqlParams);

            try
            {
                abrirLigacao();

                if (_SqlConn.State == ConnectionState.Open)
                {
                    retorno = _SqlCommand.ExecuteNonQuery();
                }
            }
            finally
            {
                fecharLigacao();
                _SqlCommand.Parameters.Clear();
            }

            return retorno;
        }

        public object executarStoredProcScalar(String sqlCmd, SqlParameter[] sqlParams)
        {
            object resultado = null;
            associarComando(sqlCmd);
            _SqlCommand.CommandType = CommandType.StoredProcedure;

            if (sqlParams != null)
                _SqlCommand.Parameters.AddRange(sqlParams);

            try
            {
                abrirLigacao();

                if (_SqlConn.State == ConnectionState.Open)
                {
                    resultado = _SqlCommand.ExecuteScalar();
                }
            }
            finally
            {
                fecharLigacao();
                _SqlCommand.Parameters.Clear();
            }

            return resultado;
        }

        public bool ColunaExiste(string tabela, string coluna)
        {
            object resultado = executarScalar(
                "SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME=@tabela AND COLUMN_NAME=@coluna",
                new SqlParameter[] {
                    new SqlParameter("@tabela", tabela),
                    new SqlParameter("@coluna", coluna)
                });
            return Convert.ToInt32(resultado) > 0;
        }

        public void GarantirEsquema()
        {
            ExecutarMigracao(@"
                IF NOT EXISTS (
                    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
                    WHERE TABLE_NAME = 'Historico_Compra' AND COLUMN_NAME = 'Id_Utilizador')
                BEGIN
                    ALTER TABLE Historico_Compra ADD Id_Utilizador int NOT NULL
                        CONSTRAINT DF_Historico_Compra_Id_Utilizador DEFAULT 0;
                END");

            ExecutarMigracao(@"
                IF NOT EXISTS (
                    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
                    WHERE TABLE_NAME = 'Compra' AND COLUMN_NAME = 'Id_Utilizador')
                BEGIN
                    ALTER TABLE Compra ADD Id_Utilizador int NOT NULL
                        CONSTRAINT DF_Compra_Id_Utilizador DEFAULT 0;
                END");

            ExecutarMigracao(@"
                IF NOT EXISTS (
                    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
                    WHERE TABLE_NAME = 'Livro' AND COLUMN_NAME = 'Stock')
                BEGIN
                    ALTER TABLE Livro ADD Stock int NOT NULL
                        CONSTRAINT DF_Livro_Stock DEFAULT 1;
                END");

            ExecutarMigracao(@"
                IF NOT EXISTS (
                    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
                    WHERE TABLE_NAME = 'HistoricoEmp' AND COLUMN_NAME = 'Duracao_Dias')
                BEGIN
                    ALTER TABLE HistoricoEmp ADD Duracao_Dias int NOT NULL
                        CONSTRAINT DF_HistoricoEmp_Duracao_Dias DEFAULT 14;
                END");

            ExecutarMigracao(@"
                IF NOT EXISTS (
                    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
                    WHERE TABLE_NAME = 'HistoricoEmp' AND COLUMN_NAME = 'Valor_Multa')
                BEGIN
                    ALTER TABLE HistoricoEmp ADD Valor_Multa int NOT NULL
                        CONSTRAINT DF_HistoricoEmp_Valor_Multa DEFAULT 0;
                END");

            ExecutarMigracao(@"
                IF NOT EXISTS (
                    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
                    WHERE TABLE_NAME = 'HistoricoEmp' AND COLUMN_NAME = 'Multa_Paga')
                BEGIN
                    ALTER TABLE HistoricoEmp ADD Multa_Paga bit NOT NULL
                        CONSTRAINT DF_HistoricoEmp_Multa_Paga DEFAULT 0;
                END");

            ExecutarMigracao(@"
                IF NOT EXISTS (
                    SELECT 1 FROM INFORMATION_SCHEMA.TABLES
                    WHERE TABLE_NAME = 'Notificacao')
                BEGIN
                    CREATE TABLE Notificacao (
                        Id int NOT NULL PRIMARY KEY,
                        Id_Utilizador int NOT NULL,
                        Id_Livro int NULL,
                        Mensagem nvarchar(500) NOT NULL,
                        Lida bit NOT NULL DEFAULT 0,
                        Data_Criacao datetime NOT NULL DEFAULT GETDATE()
                    );
                END");

            ExecutarMigracao(@"
                IF NOT EXISTS (
                    SELECT 1 FROM INFORMATION_SCHEMA.TABLES
                    WHERE TABLE_NAME = 'DevoluçãoEmp')
                BEGIN
                    CREATE TABLE [DevoluçãoEmp] (
                        Id int IDENTITY(1,1) PRIMARY KEY,
                        Id_Utilizador int NOT NULL,
                        Id_Livro int NOT NULL,
                        Data_Devolução datetime NOT NULL
                    );
                END");

            ExecutarMigracao(@"
                IF NOT EXISTS (
                    SELECT 1 FROM INFORMATION_SCHEMA.TABLES
                    WHERE TABLE_NAME = 'DevoluçãoCompra')
                BEGIN
                    CREATE TABLE [DevoluçãoCompra] (
                        Id int IDENTITY(1,1) PRIMARY KEY,
                        Id_Utilizador int NOT NULL,
                        Id_Livro int NOT NULL,
                        Data_Devolução datetime NOT NULL
                    );
                END");

            ExecutarMigracao(@"
                IF NOT EXISTS (
                    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
                    WHERE TABLE_NAME = 'Historico_Compra' AND COLUMN_NAME = 'Estado_Compra')
                BEGIN
                    ALTER TABLE Historico_Compra ADD Estado_Compra nvarchar(20) NOT NULL
                        CONSTRAINT DF_Historico_Compra_Estado_Compra DEFAULT 'Ativa';
                END");

            ExecutarMigracao(@"
                IF NOT EXISTS (
                    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
                    WHERE TABLE_NAME = 'Historico_Compra' AND COLUMN_NAME = 'Data_Devolução')
                BEGIN
                    ALTER TABLE Historico_Compra ADD Data_Devolução datetime NULL;
                END");

            ExecutarMigracao(@"
                IF NOT EXISTS (
                    SELECT 1 FROM INFORMATION_SCHEMA.TABLES
                    WHERE TABLE_NAME = 'Carteira')
                BEGIN
                    CREATE TABLE Carteira (
                        Id_Utilizador int NOT NULL PRIMARY KEY,
                        Saldo decimal(18, 2) NOT NULL CONSTRAINT DF_Carteira_Saldo DEFAULT 0
                    );
                END");
        }

        private void ExecutarMigracao(string sql)
        {
            try
            {
                executarNonQuery(sql, null);
            }
            catch
            {
                // Ignora migrações individuais que já foram aplicadas.
            }
        }

        public DataTable executarReader(String sqlCmd, SqlParameter[] sqlParams)
        {
            DataTable returnTable = new DataTable("returnTable");
            associarComando(sqlCmd);
            _SqlCommand.CommandType = CommandType.Text;

            if (sqlParams != null)
                _SqlCommand.Parameters.AddRange(sqlParams);

            try
            {
                abrirLigacao();

                if (_SqlConn.State == ConnectionState.Open)
                {
                    using (_SqlReader = _SqlCommand.ExecuteReader())
                    {
                        returnTable.Load(_SqlReader);
                    }
                }
            }
            finally
            {
                fecharLigacao();
                _SqlCommand.Parameters.Clear();
            }

            return returnTable;
        }

        public int executarNonQuery(String sqlCmd, SqlParameter[] sqlParams)
        {
            int retorno = -1;
            associarComando(sqlCmd);
            _SqlCommand.CommandType = CommandType.Text;

            if (sqlParams != null)
                _SqlCommand.Parameters.AddRange(sqlParams);

            try
            {
                abrirLigacao();

                if (_SqlConn.State == ConnectionState.Open)
                {
                    retorno = _SqlCommand.ExecuteNonQuery();
                }
            }
            finally
            {
                fecharLigacao();
                _SqlCommand.Parameters.Clear();
            }

            return retorno;
        }

        public object executarScalar(String sqlCmd, SqlParameter[] sqlParams)
        {
            object resultado = null;
            associarComando(sqlCmd);
            _SqlCommand.CommandType = CommandType.Text;

            if (sqlParams != null)
                _SqlCommand.Parameters.AddRange(sqlParams);

            try
            {
                abrirLigacao();

                if (_SqlConn.State == ConnectionState.Open)
                {
                    resultado = _SqlCommand.ExecuteScalar();
                }
            }
            finally
            {
                fecharLigacao();
                _SqlCommand.Parameters.Clear();
            }

            return resultado;
        }
    }
}