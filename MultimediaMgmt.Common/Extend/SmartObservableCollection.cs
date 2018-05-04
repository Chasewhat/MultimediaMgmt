using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MultimediaMgmt.Common.Extend
{
    /// <summary>
    /// 基于ObservableCollection实现的自定义观察者集合
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SmartObservableCollection<T> : ObservableCollection<T>
    {
        //写入锁定
        private bool IsLockUpdate = false;

        #region Construction
        /// <summary>
        /// 默认构造
        /// </summary>
        public SmartObservableCollection()
        {
        }
        /// <summary>
        /// 重载构造,基于IEnumerable创建SmartObservableCollection
        /// </summary>
        /// <param name="collection"></param>
        public SmartObservableCollection(IEnumerable<T> collection)
        {
            this.AddRange(collection);
        }
        /// <summary>
        /// 重载构造,基于List创建SmartObservableCollection
        /// </summary>
        /// <param name="collection"></param>
        public SmartObservableCollection(List<T> collection)
        {
            this.AddRange(collection);
        }

        public SmartObservableCollection(T obj)
        {
            BeginUpdate();
            this.Add(obj);
            EndUpdate();
        }
        #endregion

        #region Update
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="source"></param>
        public void AddRange(IEnumerable<T> source)
        {
            BeginUpdate();
            foreach (T item in source)
                Add(item);
            EndUpdate();
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="source"></param>
        public void RemoveRange(IEnumerable<T> source)
        {
            BeginUpdate();
            foreach (T item in source)
                Remove(item);
            EndUpdate();
        }
        /// <summary>
        /// 开启批量操作
        /// </summary>
        public virtual void BeginUpdate()
        {
            IsLockUpdate = true;
        }
        /// <summary>
        /// 批量操作完成,执行集合变化通知
        /// </summary>
        public virtual void EndUpdate()
        {
            IsLockUpdate = false;
            if (!IsLockUpdate)
                OnCollectionChanged(new System.Collections.Specialized.NotifyCollectionChangedEventArgs(System.Collections.Specialized.NotifyCollectionChangedAction.Reset));
        }
        /// <summary>
        /// 集合变化通知
        /// </summary>
        /// <param name="e"></param>
        protected override void OnCollectionChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            //如果正在执行批量操作,取消通知
            if (IsLockUpdate)
                return;
            base.OnCollectionChanged(e);
        }
        #endregion
    }
}
