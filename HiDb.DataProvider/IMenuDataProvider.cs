using HiDb.DataProvider.Dtos.Menus;
using HiDb.DataProvider.Dtos.Tables;

namespace HiDb.DataProvider
{
    public interface IMenuDataProvider
    {
        /// <summary>
        /// 获取菜单-数据库列表
        /// </summary>
        /// <returns></returns>
        public List<MenuDataBaseOutput> GetDataBaseList();

        /// <summary>
        /// 获取菜单-数据库-表列表
        /// </summary>
        /// <returns></returns>
        public List<MenuDbTableOutput> GetDbTableList();

        /// <summary>
        /// 获取菜单-数据库-视图列表
        /// </summary>
        /// <returns></returns>
        public List<MenuDbTableOutput> GetDbViewList();

        /// <summary>
        /// 获取菜单-数据库-存储过程列表
        /// </summary>
        /// <returns></returns>
        public List<MenuDbTableOutput> GetDbSpList();
    }
}