import { createRouter, createWebHashHistory } from 'vue-router'

const router = createRouter({
  history: createWebHashHistory(),
  routes: [
    {
      path: '/',
      redirect: '/login',
    },
    {
      path: '/login',
      component: () => import('../views/login/Login-Page.vue'),
    },
    {
      path: '/main',
      component: () => import('../views/main/Main-Page.vue'),
    },
  ],
})

export default router
