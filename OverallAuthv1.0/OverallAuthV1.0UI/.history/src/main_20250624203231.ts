import { createApp } from 'vue'
import { createPinia } from 'pinia'
import 'normalize.css'
import '../src/assets/CSS/index.less'
import App from './App.vue'
import router from './router'
import ElementPlus from 'element-plus'
import 'element-plus/dist/index.css'
import './assets/css/tailwind.css' // 全局引入

const app = createApp(App)

app.use(createPinia())
app.use(router)
app.use(ElementPlus)

app.mount('#app')
