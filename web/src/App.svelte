<script>
  import svelteLogo from './assets/svelte.svg'
  import viteLogo from './assets/vite.svg'
  import heroImg from './assets/hero.png'
  import carImg from './assets/car.png'
  import Counter from './lib/Counter.svelte'
  import Battery from './lib/Battery.svelte'

  let charge = $state(25)
</script>

<section id="center">
  <div class="hero">
    <img src={carImg} alt="" />
  </div>
  <div>
    <h1>Your Car</h1>
    <Battery charge={charge} />
  </div>
  <Counter />
</section>

<section id="spacer"></section>
