using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace PIS.AutoEnt.XData
{
    public interface IXObjectStorage
    {
        bool Exists(string xpath);

        XNode GetRoot();

        XNode GetSingleNode(string xpath);

        XNodeList GetNodes(string xpath);

        string GetValue(string xpath);

        void SetValue(string xpath, string value);

        void Remove(string xpath);

        XNode InsertAttr(string refpath, string name, string value, NodePosition position);

        XNode InsertEle(string refpath, string name, string value, NodePosition position);
    }
}
