import { createApp } from 'vue'
import { createPinia } from 'pinia'
import Antd from 'ant-design-vue';

import './styles/index.scss'
import 'ant-design-vue/dist/reset.css';
import './permission'
import App from './App.vue'
import router from './router'
import { errorHandler } from './error'

const app = createApp(App)
const store = createPinia()

app.use(router)
app.use(store)
errorHandler(app)
app.use(Antd).mount('#app');
