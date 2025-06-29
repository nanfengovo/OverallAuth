<template>
    <el-tabs v-model="activeTab" type="card" class="Tabs" closable @edit="handleTabsEdit" @tab-click="handleTabClick">
        <el-tab-pane v-for="tab in openedTabs" :key="tab.path" :label="tab.title" :name="tab.path"
            :closable="tab.path !== '/Home'">
            <router-view v-if="activeTab === tab.path" />
        </el-tab-pane>
    </el-tabs>
</template>

<script lang="ts" setup>
// ...原有导入和初始化代码...

// 标签页点击事件（增加刷新逻辑）
const handleTabClick = (tab: TabsPaneContext) => {
    if (activeTab.value === tab.paneName) {
        router.replace({ path: '/redirect' + tab.paneName }).then(() => {
            router.replace(tab.paneName as string)
        })
    } else {
        router.push(tab.paneName as string)
    }
}
</script>

<style scoped>
.Tabs {
    height: 100%;
    /* 确保容器高度继承 */
}

.Tabs :deep(.el-tabs__content) {
    overflow: auto;
    /* 内容溢出时滚动 */
}

.Tabs :deep(.el-tab-pane) {
    min-height: 500px;
    padding: 16px;
    position: relative;
}
</style>
