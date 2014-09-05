using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIS.AutoEnt.Data;

namespace PIS.AutoEnt.XData.DataStore
{
    public class XDataRegion : IDisposable
    {
        #region Consts

        public const string IdIndexKey = "Id";
        public const string CodeIndexKey = "Code";

        #endregion

        #region Properties

        public string Name { get; protected set; }

        #endregion

        public virtual void Dispose()
        {
        }
    }

    public class XDataRegion<T, TN> : XDataRegion
        where T : XDataItem<T>, new()
        where TN : XDataNode<T>, new()
    {

        #region Properties

        protected XDataSet<T, TN> DataSet { get; set; }

        internal EasyDictionary<IDataIndex> Indexs { get; set; }

        internal XDataIndex IdIndex
        {
            get
            {
                return Indexs.Get(IdIndexKey) as XDataIndex;
            }
        }

        internal XDataIndex CodeIndex
        {
            get
            {
                return Indexs.Get(CodeIndexKey) as XDataIndex;
            }
        }

        #endregion

        #region Constructors

        public XDataRegion(string name)
        {
            this.Name = name;

            Indexs = new EasyDictionary<IDataIndex>();

            DataSet = new XDataSet<T, TN>();

            Initialize();
        }

        protected virtual void Initialize()
        {
            this.ReloadIndexs();
        }

        #endregion

        #region Public Methods

        public virtual T NewItem(TN pNode, string code, string name = null, int? sortIndex = null, string dataType = null, object data = null)
        {
            var newItem = new T()
            {
                Code = code,
                Name = name ?? code,
                SortIndex = sortIndex ?? (pNode.Items.Count + 1),
                DataType = dataType,
                Data = data
            };

            return NewItem(pNode, newItem);
        }

        public virtual TN NewNode(TN pNode, string code, string name = null, int? sortIndex = null, string dataType = null, object data = null)
        {
            var newNode = new TN()
            {
                Code = code,
                Name = name ?? code,
                DataType = dataType,
                SortIndex = sortIndex ?? (pNode.Items.Count + 1),
                Data = data
            };

            return NewNode(pNode, newNode);
        }

        public virtual T NewItem(TN pNode, T newItem)
        {
            newItem.ParentNode = pNode;

            this.ChkPerNewItem(pNode, newItem);

            this.IdIndex.Items.Set(newItem.Id, newItem);

            pNode.Items.Add(newItem);

            return newItem;
        }

        public virtual TN NewNode(TN pNode, TN newNode)
        {
            newNode.ParentNode = pNode;

            this.ChkPerNewNode(pNode, newNode);

            this.IdIndex.Items.Set(newNode.Id, newNode);
            this.CodeIndex.Items.Set(newNode.Code, newNode);

            pNode.Nodes.Add(newNode);

            return newNode;
        }

        public TN GetNodeByCode(string code)
        {
            var node = this.CodeIndex.Get(code) as TN;

            return node;
        }

        public object GetById(Guid id)
        {
            var item = this.IdIndex.Get(id);

            return item;
        }

        public void RemoveById(Guid id)
        {
            var itemObj = this.GetById(id) as XDataItem;

            if (itemObj is TN)
            {
                var node = itemObj as TN;

                node.ParentNode.RemoveNodeByCode(itemObj.Code);

                this.IdIndex.Items.Remove(itemObj.Id);
                this.CodeIndex.Items.Remove(itemObj.Code);
            }
            else if (itemObj is T)
            {
                var item = itemObj as T;

                item.ParentNode.RemoveItemByCode(item.Code);
                this.IdIndex.Items.Remove(itemObj.Id);
            }
        }

        /// <summary>
        /// 重新加载索引，一般在XDataSet变化后择机执行
        /// </summary>
        public void ReloadIndexs()
        {
            var idIndex = DataSet.RetrieveIdIndex();
            Indexs.Set(idIndex.Name, idIndex);

            var codeIndex = DataSet.RetrieveCodeIndex();
            Indexs.Set(codeIndex.Name, codeIndex);
        }

        public TN GetRootNode(string code)
        {
            var node = this.DataSet.GetNodeByCode(code);

            return node;
        }

        public void SetRootNode(TN node)
        {
            this.DataSet.Nodes.Add(node);
        }

        #endregion

        #region XDataRegion Members

        public override void Dispose()
        {
            base.Dispose();

            this.DataSet.Nodes.Clear();
            this.DataSet = null;

            // Clear Indexs
            this.IdIndex.Items.Clear();
            this.CodeIndex.Items.Clear();

            this.Indexs.Clear();
            this.Indexs = null;
        }

        #endregion

        #region Support Methods

        protected virtual bool ChkPerNewItem(TN pNode, T newItem)
        {
            var _item = pNode.GetItemByCode(newItem.Code);

            if (_item != null)
            {
                throw new PISDataException(ErrorCode.DataKeyRepeated, "已存在编号为 " + newItem.Code + " 项 ");
            }

            return true;
        }

        protected virtual bool ChkPerNewNode(TN pNode, TN newNode)
        {
            var _node = this.GetNodeByCode(newNode.Code);

            if (_node != null)
            {
                throw new PISDataException(ErrorCode.DataKeyRepeated, "已存在编号为 " + newNode.Code + " 项 ");
            }

            return true;
        }

        #endregion
    }
}
