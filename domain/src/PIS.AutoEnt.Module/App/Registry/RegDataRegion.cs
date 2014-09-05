using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIS.AutoEnt.Repository.Interfaces;
using PIS.AutoEnt.XData.DataStore;
using PIS.AutoEnt.DataContext;

namespace PIS.AutoEnt.App
{
    public class RegDataRegion : XDataRegion<RegDataItem, RegDataNode>
    {
        #region Properties

        internal SysRegistry RegRecord
        {
            get;
            set;
        }

        #endregion

        #region Constructors

        public RegDataRegion(SysRegistry regRecord)
            : base(regRecord.Code)
        {
            RegRecord = regRecord;

            this.Reload();
        }

        #endregion

        #region XDataRegion Members

        public override RegDataItem NewItem(RegDataNode pNode, string code, string name = null, int? sortIndex = null, string dataType = null, object data = null)
        {
            var newItem = new RegDataItem()
            {
                Code = code,
                Name = name ?? code,
                SortIndex = sortIndex ?? (pNode.Items.Count + 1),
                Data = data
            };

            return this.NewItem(pNode, newItem);
        }

        public override RegDataNode NewNode(RegDataNode pNode, string code, string name = null, int? sortIndex = null, string dataType = null, object data = null)
        {
            var newNode = new RegDataNode()
            {
                Code = code,
                Name = name ?? code,
                SortIndex = sortIndex ?? (pNode.Items.Count + 1),
                Data = data
            };

            return this.NewNode(pNode, newNode);
        }

        #endregion

        #region Public Methods

        public virtual void Reload()
        {
            string datastr = RegRecord.XData;

            if (String.IsNullOrEmpty(datastr))
            {
                this.DataSet = new RegDataSet();
            }
            else
            {
                this.DataSet = CLRHelper.DeserializeFromXmlString<RegDataSet>(datastr);
            }

            this.ReloadIndexs();
        }

        public virtual SysRegistry Persist()
        {
            var type = this.DataSet.GetType();

            if (!String.IsNullOrEmpty(RegRecord.AssemblyType))
            {
                type = Type.GetType(RegRecord.AssemblyType);
            }

            string datastr = CLRHelper.SerializeToXmlString(this.DataSet);

            RegRecord.XData = datastr;

            return this.RegRecord;
        }

        #endregion
    }
}
