using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DL.Models.Service.ServiceModels
{
    public class DiaryLogNewList
    {
        public DateTime Date { get; set; }

        public string Week { get; set; }

        public decimal TotalTime { get; set; }
    }
}