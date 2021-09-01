using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsAzure.Storage.Table;

namespace tallerazure.Function.Entities
{
     public class TallerEntity : TableEntity
     {
          public DateTime CreatedTime { get; set; }

          public string NameEmployee { get; set; }

          public bool IsCompleted { get; set; }
     }
}
