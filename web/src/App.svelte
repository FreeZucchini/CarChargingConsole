<script>
  import svelteLogo from './assets/svelte.svg'
  import viteLogo from './assets/vite.svg'
  import heroImg from './assets/hero.png'
  import carImg from './assets/car.png'
  import ChargeButton from './lib/ChargeButton.svelte'
  import Battery from './lib/Battery.svelte'
  import Scheduler from './lib/Scheduler.svelte'

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
  <ChargeButton />
  <Scheduler />
</section>

<section id="spacer"></section>
