using System;

using System.Linq;



namespace dMb.Services
{
    public class QBuilder
    {
        private Query _product = new Query();

        public QBuilder()
        {
            Reset();
        }

        public Query GetQuery()
        {
            Query result = _product;

            Reset();

            return result;
        }


        public QBuilder SELECT(params string[] args)
        {
            string select = "SELECT ";

            if (args.Count() == 0)
            {
                select += "*";
                _product._select = select;
                return this;
            }

            foreach (string arg in args)
            {
                select += $"{arg}, ";
            }
            select = select.Remove(select.Length - 2); // Remove last comma

            _product._select = select;
            return this;
        }


        public QBuilder DELETE(params string[] args)
        {
            string delete = "DELETE ";

            if (args.Count() == 0)
            {
                _product._select = delete;
                return this;
            }

            foreach (string arg in args)
            {
                delete += $"{arg}, ";
            }
            delete = delete.Remove(delete.Length - 2);

            _product._select = delete;
            return this;
        }


        public QBuilder FROM(string tableName)
        {
            string from = $"FROM {tableName}";

            _product._from = from;
            return this;
        }


        public QBuilder JOIN(string rTabName, string lId, string rId)
        {
            string join = $"JOIN {rTabName} ON {lId} = {rId}";
            if (_product._join == null)
            {
                _product._join = join;
                return this;
            }


            _product._join = String.Join(" ", _product._join, join);
            return this;
        }


        public QBuilder WHERE(string condition)
        {
            string where = $"WHERE {condition}";

            _product._where = where;
            return this;
        }


        public QBuilder GROUP_BY(string columnName)
        {
            string group = $"GROUP BY {columnName}";

            _product._group = group;
            return this;
        }


        public QBuilder ORDER_BY(string columnName, bool ascending = true)
        {
            string order = $"ORDER BY {columnName} ";

            if (ascending)
            {
                order += "ASC";
            }
            else
            {
                order += "DESC";
            }

            _product._order = order;
            return this;
        }


        public QBuilder Reset()
        {
            _product = new Query();
            return this;
        }


    }



    public class Query
    {
        public Query()
        {

        }


        override
        public string ToString()
        {
            string result = string.Empty;

            result = String.Join(" ", _select, _from, _join, _where, _group, _order);


            return result;
        }


        public string _select { set; get; }
        public string _from { set; get; }
        public string _join { set; get; }
        public string _where { set; get; }
        public string _group { set; get; }
        public string _order { set; get; }


    }
}
