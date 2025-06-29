<template>
    <!-- 动态标签页容器 -->
    <el-tabs v-model="activeTab" type="card" class="Tabs" closable @edit="handleTabsEdit" @tab-click="handleTabClick">
        <!-- 自定义添加按钮图标 -->
        <template #add-icon>
            <el-icon><Select /></el-icon>
        </template>

        <!-- 动态生成标签页 -->
        <el-tab-pane v-for="tab in openedTabs" :key="tab.name" :label="tab.title" :name="tab.path"
            :closable="tab.path !== '/Home'">
            <!-- 路由视图容器 -->
            <router-view />
        </el-tab-pane>
    </el-tabs>
</template>

<script lang="ts" setup>
import { Select } from '@element-plus/icons-vue'
import type { TabPaneName, TabsPaneContext } from 'element-plus'
import { ref, watch, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'

const route = useRoute()
const router = useRouter()

// 从本地存储加载菜单数据
const menuData = ref(JSON.parse(localStorage.getItem('menu') || '[]'))

// 当前激活的标签页（绑定路由路径）
const activeTab = ref(route.path)

// 已打开的标签页列表
const openedTabs = ref([
    {
        title: '首页',
        path: '/Home',
        name: 'Home'
    }
])

// 监听路由变化（核心逻辑）
watch(() => route.path, (newPath) => {
    // 1. 查找当前路由对应的菜单项
    const menuItem = menuData.value.find((item: any) => item.url === newPath)
    if (!menuItem) return

    // 2. 检查标签页是否已存在
    const existingTab = openedTabs.value.find(tab => tab.path === newPath)

    // 3. 不存在则新增标签页
    if (!existingTab) {
        openedTabs.value.push({
            title: menuItem.describe || menuItem.name,
            path: menuItem.url,
            name: menuItem.name
        })
    }

    // 4. 激活当前标签页
    activeTab.value = newPath
}, { immediate: true })

// 标签页编辑事件（添加/删除）
const handleTabsEdit = (targetName: TabPaneName | undefined, action: 'remove' | 'add') => {
    if (action === 'remove' && targetName) {
        // 首页不可关闭
        if (targetName === '/Home') return

        // 过滤掉被关闭的标签页
        openedTabs.value = openedTabs.value.filter(tab => tab.path !== targetName)

        // 如果关闭的是当前激活页，自动跳转首页
        if (activeTab.value === targetName) {
            router.push('/Home')
        }
    }
    else if (action === 'add') {
        // 添加新标签页（示例）
        const newPath = `/new-tab-${Date.now()}`
        openedTabs.value.push({
            title: '新标签页',
            path: newPath,
            name: 'NewTab'
        })
        activeTab.value = newPath
        router.push(newPath)
    }
}

// 标签页点击事件
const handleTabClick = (tab: TabsPaneContext) => {
    router.push(tab.paneName as string)
}

// 初始化时加载已存在的标签页
onMounted(() => {
    const savedTabs = sessionStorage.getItem('openedTabs')
    if (savedTabs) {
        openedTabs.value = JSON.parse(savedTabs)
    }
})

// 标签页变化时保存状态
watch(openedTabs, (newTabs) => {
    sessionStorage.setItem('openedTabs', JSON.stringify(newTabs))
}, { deep: true })
</script>

<style scoped>
.Tabs :deep(.el-tabs__content) {
    padding: 20px;
    color: #333;
    font-size: 14px;
}
</style>
