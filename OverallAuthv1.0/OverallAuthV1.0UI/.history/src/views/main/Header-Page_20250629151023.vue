<template>
    <!-- 动态标签页容器 -->
    <el-tabs v-model="activeTab" type="card" class="Tabs" closable @edit="handleTabsEdit" @tab-click="handleTabClick">
        <!-- 自定义添加按钮图标 -->
        <template #add-icon>
            <el-icon><Select /></el-icon>
        </template>

        <!-- 动态生成标签页 -->
        <el-tab-pane v-for="tab in openedTabs" :key="tab.path" :label="tab.title" :name="tab.path"
            :closable="tab.path !== '/Home'">
            <!-- 仅激活时渲染路由视图 -->
            <!-- <router-view v-if="activeTab === tab.path" /> -->
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

// 安全加载菜单数据
const menuData = ref([] as any[])
try {
    const menuJson = localStorage.getItem('menu') || '[]'
    menuData.value = JSON.parse(menuJson)
} catch (e) {
    console.error('菜单解析失败:', e)
}

// 当前激活标签页（绑定路由路径）
const activeTab = ref(route.path)

// 已打开的标签页列表（初始化包含首页）
const openedTabs = ref([
    {
        title: '首页',
        path: '/Home',
        name: 'Home'
    }
])

// 核心：监听路由变化[3,4](@ref)
watch(() => route.path, (newPath) => {
    // 1. 同步激活状态
    activeTab.value = newPath

    // 2. 查找对应菜单项
    const menuItem = menuData.value.find((item: any) => item.url === newPath)
    if (!menuItem) return

    // 3. 不存在则新增标签页[9](@ref)
    const tabExists = openedTabs.value.some(tab => tab.path === newPath)
    if (!tabExists) {
        openedTabs.value.push({
            title: menuItem.describe || menuItem.name,
            path: menuItem.url,
            name: menuItem.name
        })
    }
}, { immediate: true })

// 标签页点击事件（路由跳转）
const handleTabClick = (tab: TabsPaneContext) => {
    router.push(tab.paneName as string)
}

// 标签页编辑（添加/删除）[1](@ref)
const handleTabsEdit = (
    targetName: TabPaneName | undefined,
    action: 'remove' | 'add'
) => {
    if (action === 'remove' && targetName) {
        // 首页保护
        if (targetName === '/Home') return

        // 移除标签页
        const updatedTabs = openedTabs.value.filter(tab => tab.path !== targetName)
        openedTabs.value = updatedTabs

        // 关闭的是当前页则跳转首页[4](@ref)
        if (activeTab.value === targetName) {
            router.push('/Home')
        }
    } else if (action === 'add') {
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

// 状态持久化[4](@ref)
onMounted(() => {
    const savedTabs = sessionStorage.getItem('openedTabs')
    if (savedTabs) {
        try {
            openedTabs.value = JSON.parse(savedTabs)
        } catch (e) {
            console.error('标签页状态恢复失败:', e)
        }
    }
})

watch(openedTabs, (newTabs) => {
    sessionStorage.setItem('openedTabs', JSON.stringify(newTabs))
}, { deep: true })
</script>

<style scoped>
.Tabs {
    /* 确保高度继承 */
    position: relative;
}

/* 深度穿透Element Plus样式 */
.Tabs :deep(.el-tabs__content) {
    padding: 20px;
    min-height: 5px;
    /* 防止高度塌陷 */
    position: relative;
}

.Tabs :deep(.el-tabs__item) {
    padding: 0 20px;
    /* 标签页标题间距 */
}
</style>
