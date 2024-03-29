import request from '@renderer/utils/request'

// 获取数据库列表
export function getDb(dbtype, name, searchTable) {
    return request({
      url: '/menu/db',
      method: 'get',
      headers: {
        'dbtype': dbtype
      },
      params: {
        name: name,
        searchTable: searchTable,
      }
    })
}

// 获取模式列表
export function getMode(database, dbtype) {
    return request({
      url: '/menu/mode',
      method: 'get',
      params: {
        database: database
      },
      headers: {
        'dbtype': dbtype
      }
    })
}
  
// 获取表列表
export function getTable(database, pageIndex, pageSize, mode, dbtype) {
    return request({
      url: '/menu/table',
      method: 'get',
      params: {
        database: database,
        mode: mode,
        pageSize: pageSize,
        pageIndex: pageIndex
      },
      headers: {
        'dbtype': dbtype
      }
    })
}
  