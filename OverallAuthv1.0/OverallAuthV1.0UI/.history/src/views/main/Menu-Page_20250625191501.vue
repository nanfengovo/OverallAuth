<template>
    <div class="Menu">
        <el-menu default-active="2" class="menu" @open="handleOpen" @close="handleClose">
            <el-menu-item index="2">
                <el-icon><icon-menu /></el-icon>
                <span>Navigator Two</span>
            </el-menu-item>
            <el-menu-item index="2">
                <el-icon><icon-menu /></el-icon>
                <span>Navigator Two</span>
            </el-menu-item>
            <el-menu-item index="2">
                <el-icon><icon-menu /></el-icon>
                <span>Navigator Two</span>
            </el-menu-item>
            <el-menu-item index="2">
                <el-icon><icon-menu /></el-icon>
                <span>Navigator Two</span>
            </el-menu-item>
        </el-menu>
    </div>
</template>
<script setup lang="ts">
import {
    Document,
    Menu as IconMenu,
    Location,
    Setting,
} from '@element-plus/icons-vue'

const handleOpen = (key: string, keyPath: string[]) => {
    console.log(key, keyPath)
}
const handleClose = (key: string, keyPath: string[]) => {
    console.log(key, keyPath)
}
</script>

<style lang="less" scoped>
.Menu {}

.menu {
    background-color: #545c64;
    height: 100vh;
}
</style>