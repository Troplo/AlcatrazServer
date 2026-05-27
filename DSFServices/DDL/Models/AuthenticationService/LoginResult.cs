using System;
using System.Collections.Generic;
using QNetZ.DDL;

namespace DSFServices.DDL.Models
{
    public class LoginResult
    {
        public Guid pidProfile { get; set; }
        public RVConnectionData pConnectionData { get; set; }
    }
}
