// 操作防抖器
export function debounce (fn: Function, delay: number, args: any){
    let timer: any = null;

    if (timer) {
        clearTimeout(timer);//如果方法多次触发，则把上次记录的延迟执行代码清掉，重新开始
    }
    timer = setTimeout(() => {
        fn(args);
    }, delay);
}