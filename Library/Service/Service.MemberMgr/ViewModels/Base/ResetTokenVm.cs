using System;

namespace Service.MemberMgr.ViewModels.Base
{
    public class ResetTokenVm
    {
        public string Token { get; set; }
        public DateTime Expire { get; set; }
    }
}
