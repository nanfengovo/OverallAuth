import { defineStore } from 'pinia'
import type { MenuItem } from '@/types' // 导入菜单类型定义
import { ref, computed } from 'vue'

// 选项式API写法（推荐）
export const useMenuStore = defineStore('menu', {
  state: () => ({
    menuItems: [] as MenuItem[], // 菜单数据数组
    lastUpdated: null as Date | null, // 最后更新时间
    isLoading: false, // 加载状态
  }),

  // 计算属性
  getters: {
    // 获取启用的菜单项
    enabledMenus: (state) => state.menuItems.filter((item) => item.isEnable),

    // 根据角色过滤菜单
    filteredMenus: (state) => {
      return (userRoles: string[]) =>
        state.menuItems.filter(
          (item) =>
            item.isEnable &&
            (item.roles.length === 0 || item.roles.some((r) => userRoles.includes(r))),
        )
    },
  },

  // 操作方法
  actions: {
    // 从API获取菜单数据
    async fetchMenus() {
      this.isLoading = true
      try {
        const response = await fetch('/api/menus')
        this.menuItems = await response.json()
        this.lastUpdated = new Date()
      } catch (error) {
        console.error('菜单加载失败', error)
      } finally {
        this.isLoading = false
      }
    },

    // 更新菜单项
    updateMenuItem(id: number, newData: Partial<MenuItem>) {
      const index = this.menuItems.findIndex((item) => item.id === id)
      if (index !== -1) {
        this.menuItems[index] = { ...this.menuItems[index], ...newData }
      }
    },

    // 重置菜单
    resetMenus() {
      this.menuItems = []
      this.lastUpdated = null
    },
  },
})

// 组合式API写法（备选）
export const useMenuSetupStore = defineStore('menuSetup', () => {
  const menuItems = ref<MenuItem[]>([])
  const lastUpdated = ref<Date | null>(null)
  const isLoading = ref(false)

  // 计算属性
  const enabledMenus = computed(() => menuItems.value.filter((item) => item.isEnable))

  const filteredMenus = computed(
    () => (userRoles: string[]) =>
      menuItems.value.filter(
        (item) =>
          item.isEnable &&
          (item.roles.length === 0 || item.roles.some((r) => userRoles.includes(r))),
      ),
  )

  // 操作方法
  async function fetchMenus() {
    isLoading.value = true
    try {
      const response = await fetch('/api/menus')
      menuItems.value = await response.json()
      lastUpdated.value = new Date()
    } catch (error) {
      console.error('菜单加载失败', error)
    } finally {
      isLoading.value = false
    }
  }

  function updateMenuItem(id: number, newData: Partial<MenuItem>) {
    const index = menuItems.value.findIndex((item) => item.id === id)
    if (index !== -1) {
      menuItems.value[index] = { ...menuItems.value[index], ...newData }
    }
  }

  function resetMenus() {
    menuItems.value = []
    lastUpdated.value = null
  }

  return {
    menuItems,
    lastUpdated,
    isLoading,
    enabledMenus,
    filteredMenus,
    fetchMenus,
    updateMenuItem,
    resetMenus,
  }
})
