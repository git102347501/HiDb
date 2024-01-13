import request from '@renderer/utils/request'

// 获取模式列表
export function getSearch(params, dbtype, cancelToken) {
    return request({
      url: '/search',
      method: 'get',
      params: params,
      headers: {
        'dbtype': dbtype
      },
      cancelToken: cancelToken
    })
}

// 执行命令
export function execute(params, dbtype, cancelToken) {
  return request({
    url: '/search/execute',
    method: 'post',
    params: params,
    headers: {
      'dbtype': dbtype
    },
    cancelToken: cancelToken
  })
}