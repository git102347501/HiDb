import request from '@renderer/utils/request'

// 获取数据库列表
export function getDb(dbtype) {
    return request({
      url: '/menu/db',
      method: 'get',
      headers: {
        'dbtype': dbtype
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
export function getTable(database, mode, dbtype) {
    return request({
      url: '/menu/table',
      method: 'get',
      params: {
        database: database,
        mode: mode
      },
      headers: {
        'dbtype': dbtype
      }
    })
}
  