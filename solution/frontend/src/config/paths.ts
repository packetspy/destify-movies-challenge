export const paths = {
  home: {
    path: '/dashboard',
    getHref: () => '/'
  },

  app: {
    root: {
      path: '/',
      getHref: () => '/dashboard'
    },
    movies: {
      path: 'movies',
      getHref: () => 'movies'
    },
    movie: {
      path: 'movies/:movieUniqueId',
      getHref: (movieUniqueId: string) => `/movies/${movieUniqueId}`
    },
    actors: {
      path: 'actors',
      getHref: () => 'actors'
    },
    actor: {
      path: 'actors/:actorUniqueId',
      getHref: (actorUniqueId: string) => `/actors/${actorUniqueId}`
    },
    directors: {
      path: 'directors',
      getHref: () => 'directors'
    },
    director: {
      path: 'directors/:directorUniqueId',
      getHref: (directorUniqueId: string) => `/directors/${directorUniqueId}`
    }
  }
} as const
