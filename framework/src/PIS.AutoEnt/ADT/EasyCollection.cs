using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace PIS.AutoEnt
{
    /// <summary>
    /// PIS collection object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    [CollectionDataContract]
    public class EasyCollection<T> : ICollection<T>
    {
        #region 成员属性

        protected IList<T> innerItems;

        public virtual int Count
        {
            get { return innerItems.Count; }
        }

        public virtual T this[int i]
        {
            get { return innerItems[i]; }
        }

        #endregion

        #region 构造函数

        public EasyCollection()
        {
            this.innerItems = new List<T>();
        }

        public EasyCollection(IEnumerable<T> items)
        {
            this.innerItems = items.ToList();
        }

        #endregion

        #region ICollection 成员

        public void Add(T item)
        {
            this.innerItems.Add(item);
        }

        public void Clear()
        {
            this.innerItems.Clear();
        }

        public bool Contains(T item)
        {
            return this.innerItems.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            this.innerItems.CopyTo(array, arrayIndex);
        }

        public bool IsReadOnly
        {
            get { return innerItems.IsReadOnly; }
        }

        public bool Remove(T item)
        {
            return this.innerItems.Remove(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.innerItems.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.innerItems.GetEnumerator();
        }

        #endregion
    }
}
