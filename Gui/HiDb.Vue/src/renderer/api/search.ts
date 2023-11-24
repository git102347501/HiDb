import request from '@renderer/utils/request'

// 获取模式列表
export function getSearch(params) {
    return request({
      url: '/search',
      method: 'get',
      params: params
    })
}

// 执行命令
export function execute(params) {
  return request({
    url: '/execute',
    method: 'post',
    params: params
  })
}