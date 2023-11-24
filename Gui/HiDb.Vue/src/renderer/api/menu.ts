import request from '@renderer/utils/request'

// 获取数据库列表
export function getDb() {
    return request({
      url: '/menu/db',
      method: 'get'
    })
}

// 获取模式列表
export function getMode(database) {
    return request({
      url: '/menu/mode',
      method: 'get',
      params: {
        database: database
      }
    })
}
  
// 获取表列表
export function getTable(database, mode) {
    return request({
      url: '/menu/table',
      method: 'get',
      params: {
        database: database,
        mode: mode
      }
    })
}
  