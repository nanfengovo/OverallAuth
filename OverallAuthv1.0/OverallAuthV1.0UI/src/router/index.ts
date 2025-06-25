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
      path: '/Main',
      component: () => import('../views/main/layout-Page.vue'),
      children: [
        {
          path: '/Home',
          component: () => import('../views/main/Main-Page.vue'),
        },
        {
          path: '/User',
          component: () => import('../views/main/User-Page.vue'),
        },
        {
          path: '/Role',
          component: () => import('../views/main/Role-Page.vue'),
        },
        {
          path: '/Menu',
          component: () => import('../views/main/MenuManage-Page.vue'),
        },
      ],
    },
  ],
})

// 动态生成路由
const generateRoutes = (menuItems: MenuItem[]) => {
  return menuItems.map((item) => ({
    path: item.url,
    name: item.name,
    component: () => import(`@/views/${item.name}.vue`), // 按需加载
    meta: {
      roles: item.roles, // 携带权限信息
    },
  }))
}

// 路由守卫
router.beforeEach((to, from, next) => {
  const userRoles = ['admin'] // 从Vuex或Pinia获取实际用户角色

  if (to.meta.roles && !to.meta.roles.some((role) => userRoles.includes(role))) {
    next('/forbidden') // 无权限跳转
  } else {
    next()
  }
})

export default router
