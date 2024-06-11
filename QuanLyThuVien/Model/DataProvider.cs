﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyThuVien.Model
{
    public class DataProvider
    {
        private static DataProvider _ins;
        public static DataProvider ins 
        { 
            get 
            {   
                if (_ins == null)   
                    _ins = new DataProvider();
                return _ins;}
            set
            {
                _ins = value;
            }
        }

        public QLTV_BETAEntities DB { get; set; }

        private DataProvider()
        {
            DB = new QLTV_BETAEntities ();
        }
    }
}