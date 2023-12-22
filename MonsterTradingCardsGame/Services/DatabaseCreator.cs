using Npgsql;

namespace MonsterTradingCardsGame.Services
{
    internal class DatabaseCreator
    {
        readonly NpgsqlDataSource DbConnection;

        public DatabaseCreator(NpgsqlDataSource dbConnection)
        {
            DbConnection = dbConnection;
        }

        public void PrepareDatabase(bool dbRefresh)
        {
            if (dbRefresh)
            {
                DropTables();
            }
            CreateUsers();
            CreateCards();
            CreatePackages();
            CreateTradingDeals();
            CreateTransactions();
        }

        private void CreateUsers()
        {
            string query = @"
            CREATE TABLE IF NOT EXISTS users
            (
                username character varying(255) NOT NULL,
                password character varying(255) NOT NULL,
                coins integer NOT NULL,
                    games integer NOT NULL,
                    elo integer NOT NULL,
                    bio character varying(255),
                image character varying(255),
                token character varying(255),
                wins integer NOT NULL,
                deck boolean NOT NULL,
                losses integer NOT NULL,
                CONSTRAINT users_pkey PRIMARY KEY (username)
            );";
            using (var cmd = DbConnection.CreateCommand(query))
            {
                cmd.ExecuteNonQuery();
            }
        }

        private void CreateCards()
        {
            string query = @"
            CREATE TABLE IF NOT EXISTS cards
            (
                cardid uuid NOT NULL,
                username character varying(255) NOT NULL,
                cardindex integer NOT NULL,
                indeck boolean NOT NULL,
                instore boolean NOT NULL,
                CONSTRAINT cards_pkey PRIMARY KEY(cardid),
                CONSTRAINT cards_username_fkey FOREIGN KEY(username)
                    REFERENCES users(username) MATCH SIMPLE
                    ON UPDATE CASCADE
                    ON DELETE CASCADE
            );";
            using (var cmd = DbConnection.CreateCommand(query))
            {
                cmd.ExecuteNonQuery();
            }
        }

        private void CreatePackages()
        {
            string query = @"
            CREATE TABLE IF NOT EXISTS packages
            (
                idone uuid NOT NULL,
                idtwo uuid NOT NULL,
                idthree uuid NOT NULL,
                idfour uuid NOT NULL,
                idfive uuid NOT NULL,
                packageid uuid NOT NULL,
                ""timestamp"" timestamp without time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
                CONSTRAINT packages_pkey PRIMARY KEY (packageid)
            );";
            using (var cmd = DbConnection.CreateCommand(query))
            {
                cmd.ExecuteNonQuery();
            }
        }

        private void CreateTradingDeals()
        {
            string query = @"
            CREATE TABLE IF NOT EXISTS tradingdeals
            (
                tradeid uuid NOT NULL,
                username character varying(255) NOT NULL,
                offeredcardid uuid NOT NULL,
                mindamage real NOT NULL,
                cardtype integer NOT NULL,
                coins integer NOT NULL,
                CONSTRAINT tradingdeals_pkey PRIMARY KEY (tradeid),
                CONSTRAINT tradingdeals_username_fkey FOREIGN KEY (username)
                    REFERENCES users (username) MATCH SIMPLE
                    ON UPDATE CASCADE
                    ON DELETE CASCADE
            );";
            using (var cmd = DbConnection.CreateCommand(query))
            {
                cmd.ExecuteNonQuery();
            }
        }

        private void CreateTransactions()
        {
            string query = @"
           CREATE TABLE IF NOT EXISTS transactions
            (
                transactionid uuid NOT NULL,
                username character varying(255) NOT NULL,
                obtainedid uuid NOT NULL,
                seller character varying(255) NOT NULL,
                soldcardid uuid NOT NULL,
                coins integer NOT NULL,
                type integer NOT NULL,
                CONSTRAINT transactions_pkey PRIMARY KEY (transactionid),
                CONSTRAINT transactions_username_fkey FOREIGN KEY (username)
                    REFERENCES users (username) MATCH SIMPLE
                    ON UPDATE CASCADE
                    ON DELETE CASCADE
            );";
            using (var cmd = DbConnection.CreateCommand(query))
            {
                cmd.ExecuteNonQuery();
            }
        }

        public void DropTables()
        {
            using (var batch = DbConnection.CreateBatch())
            {
                var batchcommand1 = new NpgsqlBatchCommand("DROP TABLE IF EXISTS users CASCADE;");
                var batchcommand2 = new NpgsqlBatchCommand("DROP TABLE IF EXISTS cards CASCADE;");
                var batchcommand3 = new NpgsqlBatchCommand("DROP TABLE IF EXISTS packages CASCADE;");
                var batchcommand4 = new NpgsqlBatchCommand("DROP TABLE IF EXISTS tradingdeals CASCADE;");
                var batchcommand5 = new NpgsqlBatchCommand("DROP TABLE IF EXISTS transactions CASCADE;");

                batch.BatchCommands.Add(batchcommand1);
                batch.BatchCommands.Add(batchcommand2);
                batch.BatchCommands.Add(batchcommand3);
                batch.BatchCommands.Add(batchcommand4);
                batch.BatchCommands.Add(batchcommand5);
                batch.ExecuteNonQuery();
            }
        }
    }
}

