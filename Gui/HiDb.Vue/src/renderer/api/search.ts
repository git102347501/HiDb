import request from '@renderer/utils/request'

// 获取模式列表
export function getSearch(params, dbtype) {
    return request({
      url: '/search',
      method: 'get',
      params: params,
      headers: {
        'dbtype': dbtype
      }
    })
}

// 执行命令
export function execute(params, dbtype) {
  return request({
    url: '/execute',
    method: 'post',
    params: params,
    headers: {
      'dbtype': dbtype
    }
  })
}