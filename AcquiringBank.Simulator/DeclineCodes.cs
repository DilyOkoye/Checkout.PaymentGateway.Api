using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AcquiringBank.Simulator
{
  
    public enum DeclineCodes
    {

        [Description("Suspected fraud")]
        SuspectedFraud,

        [Description("Bank not supported by Switch")]
        BankNotSupportedBySwitch,

        [Description("System malfunction")]
        SystemMalfunction,

        [Description("3DS authentication required")]
        StrongCustomerAuthentication


    }

}
