export const paths = {
    home: {
      path: '/dashboard',
      getHref: () => '/',
    },
    
    app: {
      root: {
        path: '/',
        getHref: () => '/dashboard',
      },
      movies: {
        path: 'movies',
        getHref: () => 'movies',
      },
      movie: {
        path: 'movies/movie/:movieUniqueId',
        getHref: (movieUniqueId: string) => `movie/${movieUniqueId}`,
      },
      actors: {
        path: 'actors',
        getHref: () => 'actors',
      },
      actor: {
        path: 'actors/actor/:actorUniqueId',
        getHref: (actorUniqueId: string) => `actor/${actorUniqueId}`,
      },
      directors: {
        path: 'directors',
        getHref: () => 'directors',
      },
    },
  } as const;