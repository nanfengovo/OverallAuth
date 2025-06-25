<script setup lang="ts">

</script>

<template>
  <div class="app">
    <router-view />
    <div class="app-footer">
      <span>© 2023-2024 <a href="https://github.com/leeroy-wang">leeroy-wang</a></span>
    </div>

  </div>
</template>

<style scoped>
#app {
  width: 100%;
  height: 100%;
  display: flex;
  flex-direction: column;
}

.app router-view {
  flex: 1;
  height: 90%;
  display: flex;
  flex-direction: column;
  padding: 20px;
}

#app .app-footer {
  text-align: center;
  padding: 10px;
  background-color: #f5f5f5;
  color: #333;
  font-size: 14px;
  margin-bottom: 20px;
}
</style>
