export type BaseEntity = {
  uniqueId?: string | null
}

export type Entity<T> = {
  [K in keyof T]: T[K]
} & BaseEntity

export type Meta = {
  page: number
  pageSize: number
  totalCount: number
  totalPages: number
}

export type Movie = Entity<{
  title: string
  year: number
  rated: string
  genre: string
  language: string
  country: string
  poster: string
  plot: string
  actors: Actor[]
  directors: Director[]
  ratings: Rating[]
}>

export type Actor = Entity<{
  name: string
  movies: Movie[]
}>

export type Director = Entity<{
  name: string
  movies: Movie[]
}>

export type Rating = Entity<{
  source: string | undefined
  value: string | undefined
}>
