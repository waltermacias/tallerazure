using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace tallerazure.Functions.Entities
{
     public class TallerEntity : TableEntity
     {
          public DateTime CreatedTime { get; set; }

          public string NameEmployee { get; set; }

          public bool IsConsolidated { get; set; }

          public bool TypeReg { get; set; }

	}
}
