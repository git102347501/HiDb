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
        public Task<List<MenuDataBaseOutput>> GetDataBaseListAsync(string? name = "",
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取菜单-数据库-表列表
        /// </summary>
        /// <returns></returns>
        public Task<List<MenuDbTableOutput>> GetDbTableListAsync(string database, string mode = "",
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取菜单-数据库-视图列表
        /// </summary>
        /// <returns></returns>
        public Task<List<MenuDbViewOutput>> GetDbViewListAsync(string database, string mode = "",
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取菜单-数据库-模式列表
        /// </summary>
        /// <param name="database"></param>
        /// <returns></returns>
        public Task<List<MenuDbModeOutput>> GetDbModeListAsync(string database,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取菜单-数据库-存储过程列表
        /// </summary>
        /// <returns></returns>
        public Task<List<MenuDbSpOutput>> GetDbSpListAsync(string database, string mode = "",
            CancellationToken cancellationToken = default);
    }
}