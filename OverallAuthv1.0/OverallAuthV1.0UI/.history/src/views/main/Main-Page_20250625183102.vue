<template>
    <div class="common-layout">
        <el-container>
            <el-aside width="200px" class="aside">Aside</el-aside>
            <el-container>
                <el-header class="header">Header</el-header>
                <el-main class="main">Main</el-main>
                <el-footer class="footer">Footer</el-footer>
            </el-container>
        </el-container>
    </div>
</template>
<script setup lang="ts">
</script>

<style lang="less" scoped>
.aside {
    background-color: #545c64;
}

.header {
    background-color: #0479ee;
}

.main {
    background-color: #e9eef3;
}

.footer {
    background-color: #e1ff00;
}
</style>