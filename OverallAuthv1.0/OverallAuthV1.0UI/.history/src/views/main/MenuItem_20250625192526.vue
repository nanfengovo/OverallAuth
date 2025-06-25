<!-- 子组件 MenuItem.vue -->
<script setup lang="ts">
import { defineProps } from 'vue'

const props = defineProps({
    item: {
        type: Object as () => MenuItem,
        required: true
    }
})
</script>

<template>
    <!-- 无子菜单 -->
    <el-menu-item v-if="!item.children || item.children.length === 0" :index="item.url">
        <el-icon>
            <component :is="item.icon" />
        </el-icon>
        <span>{{ item.name }}</span>
    </el-menu-item>

    <!-- 有子菜单 -->
    <el-sub-menu v-else :index="item.url">
        <template #title>
            <el-icon>
                <component :is="item.icon" />
            </el-icon>
            <span>{{ item.name }}</span>
        </template>
        <menu-item v-for="child in item.children" :key="child.id" :item="child" />
    </el-sub-menu>
</template>