import { createApp } from 'vue'
import { createPinia } from 'pinia'
import 'normalize.css'
import '../src/assets/CSS/index.less'
import App from './App.vue'
import router from './router'
import ElementPlus from 'element-plus'
import 'element-plus/dist/index.css'
import './assets/css/tailwind.css' // 全局引入
import * as ElementPlusIcons from '@element-plus/icons-vue'

const app = createApp(App)

app.use(createPinia())
app.use(router)
app.use(ElementPlus)

app.mount('#app')
for (const [key, component] of Object.entries(ElementPlusIcons)) {
  app.component(key, component)
}
