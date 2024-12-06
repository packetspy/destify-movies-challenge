import { Pen, Plus } from 'lucide-react'

import { Button } from '@/components/ui/button'
import { Form, FormDrawer, Input, Select, Textarea } from '@/components/ui/form'
import { useNotifications } from '@/components/ui/notifications'

import { useMovie } from '../api/get-movie'
import {
  updateMovieInputSchema,
  updateRatingInputSchema,
  useUpdateMovie
} from '../api/update-movie'
import { X } from 'lucide-react'
import { useState } from 'react'
import { Actor, Director, Rating } from '@/types/api'
import { useAllActors } from '@/features/actors/api/get-actors'
import { useAllDirectors } from '@/features/directors/api/get-directors'

type UpdateMovieProps = {
  movieUniqueId: string
}

export const UpdateMovie = ({ movieUniqueId }: UpdateMovieProps) => {
  const { addNotification } = useNotifications()
  const movieQuery = useMovie({ movieUniqueId })
  const actorAllQuery = useAllActors({})
  const directorsAllQuery = useAllDirectors({})

  const updateMovieMutation = useUpdateMovie({
    mutationConfig: {
      onSuccess: () => {
        addNotification({
          type: 'success',
          title: 'Movie Updated',
          message: 'The movie was successfull'
        })

        movieQuery.refetch()
      }
    }
  })

  const movie = movieQuery.data?.data

  const [reactiveActors, setReactiveActors] = useState(movie?.actors || [])
  const [reactiveDirectors, setReactiveDirectors] = useState(movie?.directors || [])
  const [reactiveRatings, setReactiveRatings] = useState(movie?.ratings || [])

  const [selectedActor, setSelectedActor] = useState<string | null>(null)
  const [selectedDirector, setSelectedDirector] = useState<string | null>(null)
  const [selectedRating, setSelectedRating] = useState<Rating | undefined>()

  const addActor = (uniqueId: string) => {
    if (uniqueId) {
      const actor = actorAllQuery.data?.find((actor: Actor) => actor.uniqueId === uniqueId)
      const actorExists = reactiveActors.find((actor) => actor.uniqueId === uniqueId)
      if (actor && !actorExists) setReactiveActors((prevActors) => [...prevActors, actor])
    }
    setSelectedActor(null)
  }

  const addDirector = (uniqueId: string) => {
    if (uniqueId) {
      const director = directorsAllQuery.data?.find(
        (director: Director) => director.uniqueId === uniqueId
      )
      const directorExists = reactiveDirectors.find((director) => director.uniqueId === uniqueId)
      if (director && !directorExists)
        setReactiveDirectors((prevActors) => [...prevActors, director])
    }
    setSelectedDirector(null)
  }

  const addRating = (source: string, value: string) => {
    if (source && value) {
      const rating: Rating = { source: source, value: value }
      setReactiveRatings((prevRatings) => [...prevRatings, rating])
    }
  }

  const removeActor = (uniqueId: string) => {
    setReactiveActors((prevActors) => prevActors.filter((actor) => actor.uniqueId !== uniqueId))
  }

  const removeDirector = (uniqueId: string) => {
    setReactiveDirectors((prevDirectors) =>
      prevDirectors.filter((director) => director.uniqueId !== uniqueId)
    )
  }

  const removeRating = (source: string, value: string) => {
    setReactiveRatings((prevRatings) =>
      prevRatings.filter((rating) => rating.source !== source && rating.value !== value)
    )
  }

  return (
    <FormDrawer
      isDone={updateMovieMutation.isSuccess}
      triggerButton={
        <Button size="sm" icon={<Pen className="size-4" />}>
          Update Movie
        </Button>
      }
      title="Update Movie"
      submitButton={
        <>
          <Button
            form="update-movie"
            type="submit"
            size="sm"
            isLoading={updateMovieMutation.isPending}
          >
            Submit
          </Button>
        </>
      }
    >
      <Form
        id="update-movie"
        onSubmit={(values) => {
          values.actors = reactiveActors
          values.directors = reactiveDirectors
          values.ratings = reactiveRatings
          updateMovieMutation.mutate({
            data: values,
            movieUniqueId
          })
        }}
        options={{
          defaultValues: {
            uniqueId: movie?.uniqueId ?? '',
            title: movie?.title ?? '',
            year: movie?.year ?? 0,
            rated: movie?.rated ?? '',
            genre: movie?.genre ?? '',
            language: movie?.language ?? '',
            country: movie?.country ?? '',
            poster: movie?.poster ?? '',
            plot: movie?.plot ?? '',
            actors: movie?.actors ?? [],
            directors: movie?.directors ?? [],
            ratings: movie?.ratings ?? []
          }
        }}
        schema={updateMovieInputSchema}
      >
        {({ register, formState }) => {
          return (
            <>
              <Input
                label="Title"
                error={formState.errors['title']}
                registration={register('title')}
              />

              <Input
                label="Year"
                error={formState.errors['year']}
                registration={register('year')}
                type="number"
              />

              <Input
                label="Rated"
                error={formState.errors['rated']}
                registration={register('rated')}
              />

              <Input
                label="Genre"
                error={formState.errors['genre']}
                registration={register('genre')}
              />

              <Input
                label="Language"
                error={formState.errors['language']}
                registration={register('language')}
              />

              <Input
                label="Country"
                error={formState.errors['country']}
                registration={register('country')}
              />

              <Input
                label="Poster"
                error={formState.errors['poster']}
                registration={register('poster')}
              />

              <Textarea
                label="Plot"
                error={formState.errors['plot']}
                registration={register('plot')}
              />

              <div className="border-t-2 border-gray-700 pt-2">
                <div className="grid grid-cols-2 gap-4 content-end">
                  <Select
                    label="Actors"
                    className="border border-gray-300 rounded-md"
                    defaultValue={null}
                    options={(actorAllQuery?.data ?? []).map((actor: Actor) => ({
                      value: actor.uniqueId,
                      label: actor.name
                    }))}
                    registration={register('actors', {
                      onChange: (e) => setSelectedActor(e.target.value)
                    })}
                  ></Select>
                  <div className="content-end">
                    <Button
                      type="button"
                      icon={<Plus className="size-4" />}
                      onClick={() => selectedActor && addActor(selectedActor)}
                    >
                      Add
                    </Button>
                  </div>
                </div>

                <ul className="mt-2">
                  {reactiveActors.map((actor, index) => (
                    <li key={index} className="ml-5 text-gray-700 list-none flex ">
                      {actor.name}
                      <Button
                        type="button"
                        onClick={() => removeActor(actor.uniqueId)}
                        className="content-center size-1 m-1"
                      >
                        <X className="size-4" />
                      </Button>
                    </li>
                  ))}
                </ul>
              </div>

              <div className="border-t-2 border-gray-700 pt-2">
                <div className="grid grid-cols-2 gap-4 content-end">
                  <Select
                    registration={register('directors', {
                      onChange: (e) => setSelectedDirector(e.target.value)
                    })}
                    defaultValue={selectedDirector}
                    label="Directors"
                    className="border border-gray-300 rounded-md"
                    options={(directorsAllQuery?.data ?? []).map((director: Director) => ({
                      value: director.uniqueId,
                      label: director.name
                    }))}
                  ></Select>

                  <div className="content-end">
                    <Button
                      type="button"
                      icon={<Plus className="size-4" />}
                      onClick={() => selectedDirector && addDirector(selectedDirector)}
                    >
                      Add
                    </Button>
                  </div>
                </div>

                <ul className="mt-2">
                  {reactiveDirectors.map((director, index) => (
                    <li key={index} className="ml-5 text-gray-700 list-none flex ">
                      {director.name}
                      <Button
                        type="button"
                        onClick={() => removeDirector(director.uniqueId)}
                        className="content-center size-1 m-1"
                      >
                        <X className="size-4" />
                      </Button>
                    </li>
                  ))}
                </ul>
              </div>
            </>
          )
        }}
      </Form>

      <Form
        id="insert-rating"
        onSubmit={(values: Rating) => {
          addRating(values.source, values.value)
        }}
        schema={updateRatingInputSchema}
      >
        {({ register, formState }) => {
          return (
            <>
              <div className="border-t-2 border-gray-700 pt-2">
                <label className="text-sm font-medium leading-none peer-disabled:cursor-not-allowed peer-disabled:opacity-70">
                  Ratings
                </label>
                <div className="grid grid-cols-3 gap-4 content-end flex">
                  <Input
                    label="Source"
                    error={formState.errors['source']}
                    registration={register('source')}
                  />

                  <Input
                    label="Value"
                    error={formState.errors['value']}
                    registration={register('value')}
                  />
                  <div className="content-end">
                    <Button type="submit" icon={<Plus className="size-4" />}>
                      Add
                    </Button>
                  </div>
                </div>

                <ul className="mt-2">
                  {reactiveRatings.map((rating, index) => (
                    <li key={index} className="ml-5 text-gray-700 list-none flex ">
                      {rating.source} - {rating.value}
                      <Button
                        type="button"
                        onClick={() => removeRating(rating?.source, rating.value)}
                        className="content-center size-1 m-1"
                      >
                        <X className="size-4" />
                      </Button>
                    </li>
                  ))}
                </ul>
              </div>
            </>
          )
        }}
      </Form>
    </FormDrawer>
  )
}
