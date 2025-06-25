import { createRouter, createWebHashHistory } from 'vue-router'

const router = createRouter({
  history: createWebHashHistory(),
  routes: [
    {
      path: '/',
      redirect: '/Login',
    },
    {
      path: '/Login',
      component: () => import('../views/login/Login-Page.vue'),
    },
    {
      path: '/Home',
      component: () => import('../views/main/Main-Page.vue'),
    },
  ],
})

export default router
