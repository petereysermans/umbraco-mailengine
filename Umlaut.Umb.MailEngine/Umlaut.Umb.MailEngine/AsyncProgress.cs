using System;
using System.Collections.Generic;
using System.Text;

namespace Nibble.Umb.MailEngine
{
    interface AsyncProgress
    {
        int progress();
        string message();
    }
}
