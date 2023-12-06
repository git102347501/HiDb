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
        public List<MenuDbTableOutput> GetDbTableList(string mode, string database);

        /// <summary>
        /// 获取菜单-数据库-视图列表
        /// </summary>
        /// <returns></returns>
        public List<MenuDbViewOutput> GetDbViewList(string mode, string database);

        /// <summary>
        /// 获取菜单-数据库-模式列表
        /// </summary>
        /// <param name="database"></param>
        /// <returns></returns>
        List<MenuDbModeOutput> GetDbModeList(string database);

        /// <summary>
        /// 获取菜单-数据库-存储过程列表
        /// </summary>
        /// <returns></returns>
        public List<MenuDbSpOutput> GetDbSpList(string mode, string database);
    }
}