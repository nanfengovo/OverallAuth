addIcon
<template>
    <el-tabs v-model="activeMenu" type="card" class="demo-tabs" editable @edit="handleTabsEdit">
        <template #add-icon>
            <el-icon><Select /></el-icon>
        </template>
        <el-tab-pane v-for="item in activeMenu" :key="item.name" :label="activeMenu" :name="item.name">
            {{ }}
        </el-tab-pane>
    </el-tabs>
</template>

<script lang="ts" setup>
import { Select } from '@element-plus/icons-vue'
import type { TabPaneName } from 'element-plus'
import { ref, computed } from 'vue'
import { useRoute } from 'vue-router'

const route = useRoute()
localStorage.getItem('menu') // 确保菜单数据已加载
const menu = JSON.parse(localStorage.getItem('menu') || '[]')
const menuData = ref(menu)
console.log('菜单数据11:', menuData.value)

// 获取当前激活菜单项（根据路由路径）
const activeMenu = computed(() => route.path)

let tabIndex = 2
const editableTabsValue = ref('2')
const editableTabs = ref([
    {
        title: 'Tab 1',
        name: '1',
        //content: 'Tab 1 content',
    },
    {
        title: 'Tab 2',
        name: '2',
        //content: 'Tab 2 content',
    },
])




const handleTabsEdit = (
    targetName: TabPaneName | undefined,
    action: 'remove' | 'add'
) => {
    if (action === 'add') {
        const newTabName = `${++tabIndex}`
        editableTabs.value.push({
            title: 'New Tab',
            name: newTabName,
            //content: 'New Tab content',
        })
        editableTabsValue.value = newTabName
    } else if (action === 'remove') {
        const tabs = editableTabs.value
        let activeName = editableTabsValue.value
        if (activeName === targetName) {
            tabs.forEach((tab, index) => {
                if (tab.name === targetName) {
                    const nextTab = tabs[index + 1] || tabs[index - 1]
                    if (nextTab) {
                        activeName = nextTab.name
                    }
                }
            })
        }

        editableTabsValue.value = activeName
        editableTabs.value = tabs.filter((tab) => tab.name !== targetName)
    }
}
</script>

<style>
.demo-tabs>.el-tabs__content {
    padding: 32px;
    color: #6b778c;
    font-size: 32px;
    font-weight: 600;
}
</style>
