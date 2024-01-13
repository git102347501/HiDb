import request from '@renderer/utils/request'

// 连接数据库
export function connectDb(data) {
    return request({
      url: '/datasource/connect',
      method: 'post',
      data: data,
      headers: {
        'dbtype': data.type
      }
    })
}