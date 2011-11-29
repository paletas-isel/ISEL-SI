using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PolicyDecisionPointRBAC1;

namespace PDP_TestApp
{
    class Program
    {
        //static void Main(string[] args)
        //{
        //    List<string> users = new List<string> { "U1", "U2", "U3" };

        //    List<string> roles = new List<string> { "R1", "R2", "R3", "R4" };

        //    List<string> permissions = new List<string> { "P1", "P2", "P3" };

        //    List<string> rh = new List<string> { "R1<R2", "R1<R3", "R2<R4", "R3<R4" };

        //    List<string> ua = new List<string> { "(U1,R1)", "(U2,R2)", "(U3,R3)" };

        //    List<string> pa = new List<string> { "(R1,P1)", "(R2,P2)", "(R3,P3)" };

        //    PolicyDecisionPoint pdp = new PolicyDecisionPoint(users, roles, permissions, rh, ua, pa);

        //    pdp.SavePolicy();
        //}

        static void Main(string[] args)
        {
            PolicyDecisionPoint pdp = new PolicyDecisionPoint();

            pdp.SavePolicy();
        }
    }
}
