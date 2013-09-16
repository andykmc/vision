using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using matching_test01;
using MathWorks.MATLAB.NET.Arrays;

namespace v0_1
{
    public static class MatchingHelper
    {
        public static string matchedClass;
        public static string getMatchResult()
        {
            //MessageBox.Show("match initialize");
            MatchingClass matchtest = null;
            MWCharArray output = null;
            MWArray MWresult = null;

            matchtest = new MatchingClass();
            //MessageBox.Show("start match");
            MWresult = matchtest.build_matching_test_correct_csharp2();
            output = (MWCharArray)MWresult[1, 1];
            return output.ToString();
        }
    }
}
